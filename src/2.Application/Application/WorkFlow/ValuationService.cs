using Application.Dto.ValuationService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Common;
using Domain.Common.MinimumPrice;
using Domain.Valuation;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;
using System.Globalization;

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
            };

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

            if (valuationRS.HotelBookingRulesRS?.rs?.response?.hotels != null
                && valuationRS.HotelBookingRulesRS.rs.response.hotels.hotel.Length > 0
                && valuationRS.HotelBookingRulesRS.rs.response.hotels.hotel[0].agreement != null
                && valuationRS.HotelBookingRulesRS.rs.response.hotels.hotel[0].agreement.Length > 0)
            {
                //TODO: Fill Valuation
                var response = valuationRS.HotelBookingRulesRS.rs.response;
                var hotel = response.hotels.hotel[0];
                var agreement = hotel.agreement[0];

                result.Status = StatusValuation.Available;//GetStatus(default); Ok
                result.Code = GetHotelCode(query.Include, hotel.code); //Ok
                result.Name = GetHotelName(query.Include, hotel.name); //Ok
                result.Mealplan = GetMealplan(query.Include, agreement); //Ok
                result.Price = GetPrice(query.Include, agreement); //Ok
                result.CancellationPolicy = GetCancellationPolicy(query.Include, agreement); //Ok
                result.MinimumPrice = GetMinimumPrice(query.Include, default); //Null 
                result.Fees = GetValuationFees(query.Include, default);//Null
                result.Remarks = GetRemarks(query.Include, agreement); //Ok
                result.Promotions = GetPromotions(query.Include, null); //Null
                result.Rooms = GetRooms(query.Include, agreement, query.ValuationCode);//Ok
                result.BookingCode = GetBookingCode(query.ValuationCode, response.search.number, hotel.city);
                result.PaymentType = GetPaymentType(query.Include);//Supplier
            }

            return result;
        }
        private string? GetHotelCode(Dictionary<string, List<string>>? include, uint? hotelCode)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Code.intance))
                return hotelCode.ToString();

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
            AvailHotelAgreement agreement)
        {
            if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
            {
                // TODO: Fill CancellationPolicy
                var nonRefundable = agreement.policies.Any(x => DateTime.ParseExact(x.from.Substring(0, 10), "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) <= DateTime.Now && x.percentage >= 100);

                return Services.CancellationPolicyService.GetCancellationPolicy(agreement, nonRefundable, BookingMethod.GetDeadLine);
            }

            return null;
        }

        private List<Promotion>? GetPromotions(Dictionary<string, List<string>>? include, object? hotelOption)
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

        private PaymentType? GetPaymentType(Dictionary<string, List<string>>? include)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Paymenttype.intance))
            {
                // TODO: Fill PaymentType                
                return PaymentType.SupplierPayment;
            }
            return null;
        }

        private string GetBookingCode(string vc, string searchNumber, string city)
        {
            //TODO: Fill BookingCode
            return FlowCodeServices.GetBookingCode(vc, searchNumber, city);
        }

        private IEnumerable<Domain.Common.Room>? GetRooms(Dictionary<string, List<string>>? include,
            AvailHotelAgreement agreement, string valuationCode)
        {
            // TODO: Fill Rooms
            if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Empty.intance))
            {
                var rooms = new List<Domain.Common.Room>();
                var vc = FlowCodeServices.DecodeValuationCode(valuationCode);

                var candidatesCopy = vc.RoomCandidates.ToList();

                for (int i = 0; i < agreement.room.Length; i++)
                {
                    for (int j = 1; j <= agreement.room[i].required; j++)
                    {
                        var found = candidatesCopy.FirstOrDefault(x => agreement.room[i].occupancy == x.PaxesAge.Count(x => x > ServiceConf.MaxChildAge)
                                                                        && agreement.room[i].occupancyChild == x.PaxesAge.Count(x => x > ServiceConf.MaxInfantAge && x <= ServiceConf.MaxChildAge)
                                                                        && agreement.room[i].occupancyInfant == x.PaxesAge.Count(x => x <= ServiceConf.MaxInfantAge)
                                                                        && (
                                                                                (!x.PaxesAge.Any(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge) && agreement.room[i].age == null)
                                                                                || (agreement.room[i].age != null && string.Join("-", x.PaxesAge.Where(z => z > ServiceConf.MaxInfantAge && z <= ServiceConf.MaxChildAge).OrderBy(p => p)) == string.Join("-", agreement.room[i].age.Split("-").OrderBy(m => int.Parse(m))))
                                                                           )
                                                                 );
                        if (found == null) // a veces viene una habitación con una ocupación de paxs diferente a la solicitada por lo que nno se puede reconocer el equivalente
                        {
                            if (candidatesCopy.Count() == 1) // si es una sola la habitación solicitada
                            {
                                found = candidatesCopy.FirstOrDefault(x => x.PaxesAge.Count() <= agreement.room[0].occupancy + agreement.room[0].occupancyChild + agreement.room[0].occupancyInfant); // si la room del response tiene capacidad de pax mayor a la solicitada
                                if (found == null)
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

                        var room = new Domain.Common.Room() { RoomRefId = found.RoomRefId };

                        if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Name.intance))
                            room.Name = agreement.room_type + "(" + agreement.room[i].type + ")";

                        if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Description.intance))
                            room.Description = agreement.room_type + "(" + agreement.room[i].type + ")";

                        if (IncludeService.CheckIfIsIncluded(include, Occupancy.intance, Occupancy.Empty.intance))
                            room.Occupancy = null;//GetRoomOccupancy(hotelRooms);

                        candidatesCopy.Remove(found);
                        rooms.Add(room);
                    }
                }
                return rooms;
            }
            return null;
        }

        private List<string>? GetRemarks(Dictionary<string, List<string>>? include, AvailHotelAgreement agreement)
        {
            if (IncludeService.CheckIfIsIncluded(include, Root.intance, Root.Remarks.intance))
            {
                var remarks = new List<string>();
                // TODO: Fill Remarks

                if (agreement.remarks != null && agreement.remarks.Length > 0)
                {
                    foreach (var remark in agreement.remarks)
                    {
                        if (!string.IsNullOrWhiteSpace(remark.text))
                            remarks.Add(remark.text);
                    }

                    if (remarks.Any())
                        return remarks.Distinct().ToList();
                }
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
        private Domain.Common.Mealplan? GetMealplan(Dictionary<string, List<string>>? include, AvailHotelAgreement agreement)
        {
            // TODO: Fill Mealplan
            if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Empty.intance))
            {
                var mealplan = new Domain.Common.Mealplan()
                {
                    Code = agreement.room_basis
                };

                if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Name.intance))
                    mealplan.Name = agreement.room_basis;

                return mealplan;
            }

            return null;
        }

        private Domain.Common.Price.Price? GetPrice(Dictionary<string, List<string>>? include, AvailHotelAgreement agreement)
        {
            if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
            {
                // TODO: Fill Price
                return PriceService.GetPrice(agreement.currency, agreement.total, false, null, null);
            }

            return null;
        }

        private MinimumPrice? GetMinimumPrice(Dictionary<string, List<string>>? include, object hotelOption)
        {
            // TODO: Fill MinimumPrice
            if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
            {
                return null;//GetMinimumPrice(0, "");
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
