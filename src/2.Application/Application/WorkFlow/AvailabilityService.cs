using Application.Dto.AvailabilityService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Availability;
using Domain.Common;
using Domain.Common.MinimumPrice;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;
using System.Text;
using AvailabilityRS = Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS.AvailabilityRS;

namespace Application.WorkFlow
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IConnector _connector;

        public AvailabilityService(
            IConnector connector)
        {
            _connector = connector;
        }

        public class GroupedByMealplan
        {
            public string? Name { get; set; }
            public required List<Combination> Combinations { get; set; }
        }

        public async Task<Availability> GetAvailability(AvailabilityQuery query)
        {
            var availabilities = await _connector.Availability(ConvertToConnectoryQuery(query));
            return ToDto(query, availabilities);
        }

        private AvailabilityConnectorQuery ConvertToConnectoryQuery(AvailabilityQuery query)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var rooms = new Infrastructure.Connectivity.Queries.Room[query.SearchCriteria.RoomCandidates.Count];
            var index = 0;

            foreach (var room in query.SearchCriteria.RoomCandidates)
            {
                rooms[index] = new Infrastructure.Connectivity.Queries.Room()
                {
                    RoomRefId = room.RoomRefId,
                    Pax = room.PaxesAge.Select(x => new Infrastructure.Connectivity.Queries.Base.Pax() { Age = x }).ToList()
                };
                index++;
            }

            var searchCriteria = new Infrastructure.Connectivity.Queries.SearchCriteria
            {
                CheckInDate = query.SearchCriteria.CheckIn,
                CheckOutDate = query.SearchCriteria.CheckOut,
                Currency = query.SearchCriteria.Currency,
                Nationality = query.SearchCriteria.Nationality,
                Market = query.SearchCriteria.Market,
                RoomCandidates = rooms,
                Accommodations = query.SearchCriteria.AccommodationCodes.ToList(),
                Language = query.SearchCriteria.Language,
                OnRequest = query.SearchCriteria.OnRequest
            };


            var connectorQuery = new AvailabilityConnectorQuery
            {
                SearchCriteria = searchCriteria,
                ConnectionData = new ConnectionData()
                {
                    Url = connection.Url,
                    User = connection.User,
                    Password = connection.Password,
                    Actor = connection.Actor,
                },
                AdvancedOptions = new AvConnectorAdvancedOptions()
                {
                    ShowBreakdownPrice = GetShowPriceBreakDown(query.Include),
                    ShowCancellationPolicies = GetShowCancellationPolicies(query.Include),
                    ShowHotelInfo = GetShowHotelInfo(query.Include)
                },
                AuditRequests = query.AuditRequests,
                Timeout = query.Timeout
            };
            return connectorQuery;
        }

        private Availability ToDto(AvailabilityQuery query, (AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData) Availabilities)
        {
            var availability = new Availability
            {
                AuditData = Availabilities.AuditData
            };

            if (Availabilities.Errors != null && Availabilities.Errors.Any())
                availability.Errors = Availabilities.Errors;
            else
                availability.Accommodations = WithResults(query, Availabilities.AvailabilityRs);

            return availability;
        }

        private List<Accommodation> WithResults(AvailabilityQuery query, AvailabilityRS? AvailabilityRs)
        {
            var vc = new StringBuilder();

            var accommodations = new List<Accommodation>();
            if (AvailabilityRs != null)
            {
                //foreach (var hotelResult in AvailabilityRs.availHotels)
                //{
                var mealPlan = new Dictionary<string, GroupedByMealplan>();
                //foreach (var hotelOption in hotelResult.availRoomRates)
                //{
                //    if (!string.IsNullOrWhiteSpace(hotelOption.mealPlan))
                //    {
                var combination = new Combination
                {
                    PaymentType = GetPaymentType(default(string)),
                    Status = StatusAvailability.Available,
                    NonRefundable = IsNotRefundable(default),
                    Rooms = GetRooms(query.Include, default),
                    Fees = GetFees(query.Include, default),
                    MinimumPrice = GetMinimumPrice(default),
                    Price = GetPrice(default(Object)),
                    ValuationCode = GetValuationCode(vc, default),
                    CancellationPolicy = GetCancellationPolicy(query.Include, query.SearchCriteria.CheckIn, default),
                    RateConditions = GetRateConditions(query.Include, default)
                };
                //if (hotelOption.AdditionalElements != null)
                //{
                //    combination.Remarks = GetRemarks(query.Include, hotelOption.AdditionalElements.HotelSupplements);
                //    combination.Promotions = GetPromotions(query.Include, hotelOption.AdditionalElements.HotelOffers);
                //}

                AddMealplan(mealPlan, combination, "", query);
                //    }
                //}
                var accommodation = GetAccommodation(query.Include, default, default, mealPlan);
                if (accommodation.Mealplans != null && accommodation.Mealplans.Any())
                    accommodations.Add(accommodation);
                //}
            }
            return accommodations;

        }

        private IList<RateConditionType>? GetRateConditions(Dictionary<string, List<string>>? include, object value)
        {
            if (IncludeService.CheckIfIsIncluded(include, RateConditions.intance, RateConditions.Empty.intance))
            {
                var result = new List<RateConditionType>();

                // Si la combinacion es Nonrefundable se debe agregar  este RateConditionType
                //result.Add(RateConditionType.NonRefundable);

                // Si el la combinacion es para Paquete (PACKAGE) se debe agregar este RateConditionType
                //result.Add(RateConditionType.Package);

                if (result.Any())
                    return result;
            }
            ;

            return null;
        }

        private Accommodation GetAccommodation(Dictionary<string, List<string>>? include, string code, string? name, Dictionary<string, GroupedByMealplan> mealPlan)
        {
            var accommodation = new Accommodation
            {
                Code = code
            };

            if (IncludeService.CheckIfIsIncluded(include, Accommodations.intance, Accommodations.Name.intance))
                accommodation.Name = name;

            accommodation.Mealplans = GetMealplans(include, mealPlan);
            return accommodation;
        }

        private PaymentType GetPaymentType(string? paymentType)
        {
            if (paymentType == null)
                return PaymentType.SupplierPayment;

            if (paymentType != null && paymentType == "")
                return PaymentType.PaymentAtDestination;

            return PaymentType.SupplierPayment;
        }

        private bool IsNotRefundable(object cancelpolicy)
        {
            return false;
        }


        private string GetValuationCode(StringBuilder vc, object data)
        {

            return FlowCodeServices.GetValuationCode(vc, data);
        }



        private List<Domain.Common.Fee>? GetFees(Dictionary<string, List<string>>? include, object hotelOption)
        {
            if (IncludeService.CheckIfIsIncluded(include, Fees.intance, Fees.Empty.intance))
            {
                var fees = new List<Domain.Common.Fee>();

                if (fees.Any())
                    return fees;
            }

            return null;
        }

        private Domain.Common.Price.Price GetPrice(object prices)
        {
            var price = PriceService.GetPrice("", 0, false, null, null); ;

            return price;
        }

        private MinimumPrice? GetMinimumPrice(object prices)
        {
            return null;
        }

        private IList<Domain.Common.Room>? GetRooms(Dictionary<string, List<string>>? include, object hotelRooms)
        {
            var rooms = new List<Domain.Common.Room>();

            for (var i = 0; i < 1; i++)
            {
                var room = new Domain.Common.Room
                {
                    RoomRefId = i + 1,
                    Code = ""
                };
                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Occupancy.intance))
                    room.Occupancy = GetRoomOccupancy(hotelRooms);
                rooms.Add(room);
            }

            return rooms;
        }

        private Domain.Common.RoomOccupancy? GetRoomOccupancy(object room)
        {
            if (room != null)
            {
                return new Domain.Common.RoomOccupancy()
                {
                    Adults = 1,
                    Children = 1,
                    AvailableRooms = 1,
                };
            }
            else
                return null;
        }

        private Domain.Common.CancellationPolicy.CancellationPolicy? GetCancellationPolicy(Dictionary<string, List<string>>? include
            , DateTime checkIn, object? cancellationPolicy)
        {
            if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
            {
                if (cancellationPolicy != null)
                    return Services.CancellationPolicyService.GetCancellationPolicy(cancellationPolicy, 0, "", checkIn);
            }
            return null;
        }




        private IList<Domain.Availability.Mealplan> GetMealplans(Dictionary<string, List<string>>? include,
            Dictionary<string, GroupedByMealplan> mealPlans)
        {
            var accommodationsMealplan = new List<Domain.Availability.Mealplan>();
            foreach (var mealplanKey in mealPlans.Keys)
            {
                var mealplan = new Domain.Availability.Mealplan
                {
                    Code = mealplanKey,
                    Combinations = mealPlans[mealplanKey].Combinations
                };

                if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Name.intance))
                    mealplan.Name = mealPlans[mealplanKey].Name;

                accommodationsMealplan.Add(mealplan);
            }

            return accommodationsMealplan;
        }

        private void AddMealplan(Dictionary<string, GroupedByMealplan> mealplans, Combination combination, string board, AvailabilityQuery query)
        {
            if (IsAValidCombination(combination, query))
            {
                if (mealplans.ContainsKey(board))
                    mealplans[board].Combinations.Add(combination);
                else
                {
                    var groupedByMealplan = new GroupedByMealplan() { Name = board, Combinations = [] };
                    groupedByMealplan.Combinations.Add(combination);
                    mealplans.Add(board, groupedByMealplan);
                }
            }
        }

        private bool IsAValidCombination(Combination combination, AvailabilityQuery query)
        {
            if ((combination.Rooms == null || !combination.Rooms.Any()) || query.SearchCriteria.RoomCandidates.Count() != combination.Rooms.Count())
                return false;

            if (combination.Price == null)
                return false;

            return true;

        }

        private bool GetShowHotelInfo(Dictionary<string, List<string>>? include)
        {
            return IncludeService.CheckIfIsIncluded(include, Accommodations.intance, Accommodations.Empty.intance) ||
                   IncludeService.CheckIfIsIncluded(include, Accommodations.intance, Accommodations.Name.intance);
        }

        private bool GetShowCancellationPolicies(Dictionary<string, List<string>>? include)
        {
            return IncludeService.CheckIfIsIncluded(include, new Cancellationpolicy(), new Cancellationpolicy.Empty());
        }

        private bool GetShowPriceBreakDown(Dictionary<string, List<string>>? include)
        {
            return IncludeService.CheckIfIsIncluded(include, new Fees(), new Fees.Empty());
        }


    }
}
