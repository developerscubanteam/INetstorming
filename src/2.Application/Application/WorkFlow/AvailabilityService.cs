using Application.Dto.AvailabilityService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Availability;
using Domain.Common;
using Domain.Common.MinimumPrice;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;
using System.Globalization;
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
                var response = AvailabilityRs.rs.response;
                foreach (var hotel in response.hotels.hotel)
                {
                    var mealPlan = new Dictionary<string, GroupedByMealplan>();
                    var allBoards = hotel.agreement.Select(x => x.room_basis).Distinct();

                    foreach (var board in allBoards)
                    {
                        if (!string.IsNullOrWhiteSpace(board))
                        {
                            var validAgreements = hotel.agreement.Where(x => x.room_basis == board);
                            foreach (var agreement in validAgreements)
                            {
                                var nonRefundable = IsNotRefundable(agreement);

                                var combination = new Combination
                                {
                                    PaymentType = PaymentType.SupplierPayment, //ok
                                    Status = agreement.available ? StatusAvailability.Available : StatusAvailability.OnRequest,//Ok
                                    NonRefundable = nonRefundable,//Ok
                                    Rooms = GetRooms(query.Include, agreement, query.SearchCriteria.RoomCandidates),//Ok
                                    Fees = GetFees(query.Include, default),
                                    MinimumPrice = GetMinimumPrice(default),
                                    Price = GetPrice(agreement),//Ok
                                    ValuationCode = GetValuationCode(vc, hotel.code.ToString(), response, query, agreement),
                                    CancellationPolicy = GetCancellationPolicy(query.Include, agreement, nonRefundable.GetValueOrDefault(), BookingMethod.Availability),//Ok
                                    RateConditions = GetRateConditions(query.Include, agreement, nonRefundable.GetValueOrDefault()), //Ok
                                    Remarks = GetAvailRemarks(query.Include, agreement) //Ok
                                };

                                AddMealplan(mealPlan, combination, board, query);
                            }
                        }
                    }
                    var accommodation = GetAccommodation(query.Include, hotel.code.ToString(), hotel.name, mealPlan);
                    if (accommodation.Mealplans != null && accommodation.Mealplans.Any())
                        accommodations.Add(accommodation);
                }
            }
            return accommodations;

        }

        private IList<RateConditionType>? GetRateConditions(Dictionary<string, List<string>>? include,
            AvailHotelAgreement agreement, bool nonRefundable)
        {
            if (IncludeService.CheckIfIsIncluded(include, RateConditions.intance, RateConditions.Empty.intance))
            {
                if (nonRefundable)
                {
                    var result = new List<RateConditionType>
                    {
                        RateConditionType.NonRefundable
                    };
                    if (result.Any())
                        return result;
                }

                // Si la combinacion es Nonrefundable se debe agregar  este RateConditionType
                //result.Add(RateConditionType.NonRefundable);

                // Si el la combinacion es para Paquete (PACKAGE) se debe agregar este RateConditionType
                //result.Add(RateConditionType.Package);

            }
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

        private bool? IsNotRefundable(AvailHotelAgreement agreement)
        {
            return agreement.policies?.Any(x => DateTime.ParseExact(x.from.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) <= DateTime.Now && x.percentage >= 100); ;
        }


        private string GetValuationCode(StringBuilder vc, string propertyId,
            AvailEnvelopeResponse response, AvailabilityQuery query, AvailHotelAgreement agreement)
        {
            var roomCandidatesList = GetValRooms(agreement, query.SearchCriteria.RoomCandidates);
            return roomCandidatesList != null
                ? FlowCodeServices.GetValuationCode(vc, propertyId, agreement.id, agreement.total,
                    response.search.number, roomCandidatesList,
                    query.SearchCriteria.CheckIn.ToFormat_yyyyMMdd(),
                    query.SearchCriteria.CheckOut.ToFormat_yyyyMMdd(),
                    query.SearchCriteria.Nationality,
                    query.Timeout)
                : string.Empty;
        }

        private List<RoomCandidates> GetValRooms(AvailHotelAgreement agreement, ICollection<Dto.AvailabilityService.Room> roomCandidates)
        {
            var result = new List<RoomCandidates>();
            var candidatesCopy = roomCandidates.ToList();

            foreach (var item in agreement.room)
            {
                for (int i = 1; i <= item.required; i++)
                {
                    var matchRoom = candidatesCopy.FirstOrDefault(x => item.occupancy == x.PaxesAge.Count(x => x > ServiceConf.MaxChildAge) // cantidad de adultos
                                                                    && item.occupancyChild == x.PaxesAge.Count(x => x > ServiceConf.MaxInfantAge && x <= ServiceConf.MaxChildAge) // cantidad de CHD
                                                                    && item.occupancyInfant == x.PaxesAge.Count(x => x <= ServiceConf.MaxInfantAge) // cantidad de INF
                                                                    && (
                                                                         (!x.PaxesAge.Any(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge) && item.age == null) // que no haya CHD en la habitación y no vengan edades en la solicitud
                                                                                                                                                                                  // o que vengan edades de CHD en el response y coincidan con las solicitadas
                                                                         || (item.age != null && string.Join("-", x.PaxesAge.Where(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge).OrderBy(p => p)) == string.Join("-", item.age.Split("-").OrderBy(m => int.Parse(m))))
                                                                       )
                    );

                    if (matchRoom == null) // a veces viene una habitación con una ocupación de paxs diferente a la solicitada por lo que nno se puede reconocer el equivalente
                    {
                        if (candidatesCopy.Count() == 1) // si es una sola la habitación solicitada
                        {

                            matchRoom = candidatesCopy.FirstOrDefault(x => x.PaxesAge.Count() <= agreement.room[0].occupancy + agreement.room[0].occupancyChild + agreement.room[0].occupancyInfant); // si la room del response tiene capacidad de pax mayor a la solicitada
                            if (matchRoom == null)
                            {
                                return null;
                            }
                        }
                        else // si había más de una habitación solicitada, no se puede reconocer la equivalencia
                        {
                            return null;
                        }
                    }
                    ;

                    var room = new RoomCandidates()
                    {
                        RoomRefId = matchRoom.RoomRefId,
                        PaxesAge = matchRoom.PaxesAge,
                        RoomType = item.type,
                        Occupancy = item.occupancy,
                        Edad = item.age,
                        Extrabed = item.extrabed,
                        Cot = item.cot,
                    };
                    result.Add(room);
                    candidatesCopy.Remove(matchRoom);
                }
                ;
            }
            return result;
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

        private Domain.Common.Price.Price GetPrice(AvailHotelAgreement agreement)
        {
            return PriceService.GetPrice(agreement.currency, agreement.total, false, null, null);
        }

        private MinimumPrice? GetMinimumPrice(object prices)
        {
            return null;
        }

        private IList<Domain.Common.Room>? GetRooms(Dictionary<string, List<string>>? include,
            AvailHotelAgreement? agreement, ICollection<Dto.AvailabilityService.Room> roomCandidates)
        {
            var rooms = new List<Domain.Common.Room>();
            var candidatesCopy = roomCandidates.ToList();

            for (int i = 0; i < agreement.room.Length; i++)
            {
                for (int j = 1; j <= agreement.room[i].required; j++)
                {
                    var matchRoom = candidatesCopy.FirstOrDefault(x => agreement.room[i].occupancy == x.PaxesAge.Count(x => x > ServiceConf.MaxChildAge)
                                                                    && agreement.room[i].occupancyChild == x.PaxesAge.Count(x => x > ServiceConf.MaxInfantAge && x <= ServiceConf.MaxChildAge)
                                                                    && agreement.room[i].occupancyInfant == x.PaxesAge.Count(x => x <= ServiceConf.MaxInfantAge)
                                                                    && (
                                                                         (!x.PaxesAge.Any(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge) && agreement.room[i].age == null)
                                                                         || (agreement.room[i].age != null && string.Join("-", x.PaxesAge.Where(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge).OrderBy(p => p)) == string.Join("-", agreement.room[i].age.Split("-").OrderBy(m => int.Parse(m))))
                                                                       )
                    );

                    if (matchRoom == null) // a veces viene una habitación con una ocupación de paxs diferente a la solicitada por lo que nno se puede reconocer el equivalente
                    {
                        if (candidatesCopy.Count() == 1) // si es una sola la habitación solicitada
                        {
                            matchRoom = candidatesCopy.FirstOrDefault(x => x.PaxesAge.Count() <= agreement.room[0].occupancy + agreement.room[0].occupancyChild + agreement.room[0].occupancyInfant); // si la room del response tiene capacidad de pax mayor a la solicitada
                            if (matchRoom == null)
                            {
                                return null;
                            }
                        }
                        else // si había más de una habitación solicitada, no se puede reconocer la equivalencia
                        {
                            return null;
                        }
                    }
                    ;

                    var room = new Domain.Common.Room()
                    {
                        Code = agreement.room_type + "(" + agreement.room[i].type + ")",
                        Description = agreement.room_type + "(" + agreement.room[i].type + ")",
                        RoomRefId = matchRoom.RoomRefId
                    };

                    if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Occupancy.intance))
                        room.Occupancy = null;//GetRoomOccupancy(hotelRooms);

                    candidatesCopy.Remove(matchRoom);
                    rooms.Add(room);
                }
                ;
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

        private Domain.Common.CancellationPolicy.CancellationPolicy? GetCancellationPolicy(Dictionary<string, List<string>>? include,
            AvailHotelAgreement? agreement, bool nonRefundable, BookingMethod method)
        {
            if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
            {
                return Services.CancellationPolicyService.GetCancellationPolicy(agreement, nonRefundable, method);
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

        public List<string>? GetAvailRemarks(Dictionary<string, List<string>>? include,
            AvailHotelAgreement agreement)
        {
            if (IncludeService.CheckIfIsIncluded(include, Remarks.intance, Remarks.Empty.intance))
            {
                var remarkList = new List<string>();
                if (agreement.remarks != null && agreement.remarks.Length > 0)
                {
                    foreach (var remark in agreement.remarks)
                    {
                        if (!string.IsNullOrWhiteSpace(remark.text))
                            remarkList.Add(remark.text);
                    }
                }

                if (remarkList.Any())
                    return remarkList;
            }

            return null;
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
