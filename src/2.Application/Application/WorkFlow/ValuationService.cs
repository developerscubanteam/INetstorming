using Application.Dto.ValuationService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Common;
using Domain.Common.MinimumPrice;
using Domain.Valuation;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;

namespace Application.WorkFlow
{
    public class ValuationService : IValuationService
    {
        private readonly IConnector _connector;

        public ValuationService(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<Valuation> GetValuation(ValuationQuery query)
        {
            var valuation = await CallConnector(query);
            return valuation;
        }

        private async Task<Valuation> CallConnector(ValuationQuery query)
        {
            var valuation = new Valuation();
            try
            {
                var valuationCode = FlowCodeServices.DecodeValuationCode(query.ValuationCode);

                var valuationRs = await _connector.Valuation(ConvertToConnectoryQuery(query, valuationCode));

                valuation = ToDto(valuationRs, valuationCode, query);

                return valuation;
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), Domain.Error.ErrorType.Error, Domain.Error.CategoryErrorType.Provider);
                valuation.Errors = [error];
            }
            ;
            return valuation;

        }

        private ValuationConnectorQuery ConvertToConnectoryQuery(ValuationQuery query, ValuationCode vc)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var connectionData = new ConnectionData()
            {
                Url = connection.Url,
                User = connection.User,
                Password = connection.Password,
                Actor = connection.Actor,
            };

            var connectorQuery = new ValuationConnectorQuery()
            {
                AdvancedOptions = new VAConnectorAdvancedOptions()
                {
                    ShowBreakdownPrice = GetShowPriceBreakDown(query.Include)
                },
                ConnectionData = connectionData,
                ValuationCode = vc,
                Timeout = query.Timeout
            };

            return connectorQuery;
        }
        private bool GetShowPriceBreakDown(Dictionary<string, List<string>>? include)
        {
            return IncludeService.CheckIfIsIncluded(include, Fees.intance, Fees.Empty.intance);
        }

        private Valuation ToDto((ValuationRS? HotelBookingRulesRS, List<Domain.Error.Error>? Errors, AuditData AuditData) valuationRS, ValuationCode vc, ValuationQuery query)
        {
            var result = new Valuation
            {
                AuditData = valuationRS.AuditData
            };

            if (valuationRS.Errors != null && valuationRS.Errors.Any())
                result.Errors = valuationRS.Errors;

            if (valuationRS.HotelBookingRulesRS != null)
            {
                //TODO: Fill Valuation
                //var hotelOption = Data = default;
                result.Status = GetStatus(default);
                result.Code = GetHotelCode(query.Include, default);
                result.Name = GetHotelName(query.Include, default);
                result.Mealplan = GetMealplan(query.Include, default);
                result.Price = GetPrice(query.Include, default);
                result.CancellationPolicy = GetCancellationPolicy(query.Include, DateTime.Parse(vc.CheckIn), default);
                result.MinimumPrice = GetMinimumPrice(query.Include, default);
                result.Fees = GetValuationFees(query.Include, default);
                result.Remarks = GetRemarks(query.Include, default);
                result.Promotions = GetPromotions(query.Include, default);
                result.Rooms = GetRooms(query.Include, default);
                result.BookingCode = GetBookingCode(query.ValuationCode, default);
                result.PaymentType = GetPaymentType(query.Include, default, vc.CheckIn);
            }

            return result;
        }
        private string? GetHotelCode(Dictionary<string, List<string>>? include, string? hotelCode)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Code.intance))
                return hotelCode;

            return null;
        }
        private string? GetHotelName(Dictionary<string, List<string>>? include, string? name)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Name.intance))
                return name;

            return null;
        }

        private StatusValuation GetStatus(object hotelOption)
        {
            //TODO: Fill Status
            // return hotelOption.Status == null || hotelOption.Status == "OK" ? StatusValuation.Available : StatusValuation.OnRequest;
            return StatusValuation.Available;
        }

        private Domain.Common.CancellationPolicy.CancellationPolicy? GetCancellationPolicy(Dictionary<string, List<string>>? include,
            DateTime checkIn, object? cancellationPolicy)
        {
            if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
            {
                // TODO: Fill CancellationPolicy
                if (cancellationPolicy != null)
                    return Services.CancellationPolicyService.GetCancellationPolicy(default, default, default);
            }

            return null;
        }

        private List<Promotion>? GetPromotions(Dictionary<string, List<string>>? include, object hotelOption)
        {
            if (IncludeService.CheckIfIsIncluded(include, Promotions.intance, Promotions.Empty.intance))
            {
                if (hotelOption != null)
                {
                    var promotions = new List<Promotion>();
                    // TODO: Fill Promotions
                    return promotions;
                }
            }
            return null;
        }

        private PaymentType? GetPaymentType(Dictionary<string, List<string>>? include, object hotelOption, string checkIn)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Paymenttype.intance))
            {
                // TODO: Fill PaymentType
                //if (hotelOption.PaymentDestination == true)
                //    return PaymentType.PaymentAtDestination;

                //if (hotelOption.VirtualCreditCardInfo != null)
                //    if (DateTime.Parse(checkIn).Date == DateTime.Parse(hotelOption.VirtualCreditCardInfo.ValidFrom).Date)
                //        return PaymentType.CardCheckIn;
                //    else
                //        return PaymentType.CardBooking;
                //else
                return PaymentType.SupplierPayment;
            }
            return null;
        }

        private string GetBookingCode(string vc, object hotelOption)
        {
            //TODO: Fill BookingCode
            return FlowCodeServices.GetBookingCode(vc, "", [0], [0]);
        }

        private IEnumerable<Domain.Common.Room>? GetRooms(Dictionary<string, List<string>>? include, object hotelRooms)
        {
            // TODO: Fill Rooms
            if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Empty.intance))
            {
                var rooms = new List<Domain.Common.Room>();
                //foreach (var hotelRoom in hotelRooms)
                //{

                var room = new Domain.Common.Room() { RoomRefId = 1 };
                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Name.intance))
                    room.Name = "";

                if (IncludeService.CheckIfIsIncluded(include, Occupancy.intance, Occupancy.Empty.intance))
                    room.Occupancy = GetRoomOccupancy(hotelRooms);

                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Description.intance))
                    room.Description = "hotelRoom.Description";

                return rooms;
            }
            return null;
        }

        private List<string>? GetRemarks(Dictionary<string, List<string>>? include, object hotelOption)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Remarks.intance))
            {
                var remarks = new List<string>();
                // TODO: Fill Remarks

                if (remarks.Any())
                    return remarks;

            }
            return null;
        }

        private Domain.Common.RoomOccupancy? GetRoomOccupancy(object? room)
        {
            if (room != null)
            {
                return new Domain.Common.RoomOccupancy()
                {
                    // TODO: Fill RoomOccupancy
                };
            }
            else
                return null;
        }
        private Domain.Common.Mealplan? GetMealplan(Dictionary<string, List<string>>? include, object hotelOption)
        {
            // TODO: Fill Mealplan
            if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Empty.intance))
            {
                if (hotelOption != null)
                {
                    var mealplan = new Domain.Common.Mealplan()
                    {
                        Code = "hotelOption.PriceInformation.Board.Type,"
                    };

                    if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Name.intance))
                        mealplan.Name = "hotelOption.PriceInformation.Board.Text;";

                    return mealplan;
                }
            }

            return null;
        }

        private Domain.Common.Price.Price? GetPrice(Dictionary<string, List<string>>? include, object hotelOption)
        {
            if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
            {
                // TODO: Fill Price
                return PriceService.GetPrice(default, default, default, default, null);
            }

            return null;
        }

        private MinimumPrice? GetMinimumPrice(Dictionary<string, List<string>>? include, object hotelOption)
        {
            // TODO: Fill MinimumPrice
            if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
            {

                return GetMinimumPrice(0, "");

            }

            return null;
        }

        private MinimumPrice? GetMinimumPrice(decimal? totalSellingPrice, string currency)
        {
            if (totalSellingPrice.HasValue)
            {
                var minimumPrice = new Domain.Common.MinimumPrice.MinimumPrice()
                {
                    Purchase = new Domain.Common.MinimumPrice.Purchase()
                    {
                        Amount = totalSellingPrice.Value,
                        Currency = currency
                    }
                };
                return minimumPrice;
            }
            return null;
        }

        private IEnumerable<Fee>? GetValuationFees(Dictionary<string, List<string>>? include, object hotelOption)
        {
            if (IncludeService.CheckIfIsIncluded(include, Fees.intance, Fees.Empty.intance))
            {

                var fees = new List<Fee>();
                // TODO: Fill Fees

                if (fees.Any())
                    return fees;
            }
            return null;
        }
    }
}
