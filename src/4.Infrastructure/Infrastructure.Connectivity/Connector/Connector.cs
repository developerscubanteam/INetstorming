using Domain.Common;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using AvailabilityRq = Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;



namespace Infrastructure.Connectivity.Connector
{
    public class Connector : IConnector
    {
        private readonly IHttpWrapper _httpWrapper;
        public Connector(IHttpWrapper httpWrapper)
        {
            _httpWrapper = httpWrapper;
        }

        public async Task<(AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData)> Availability(AvailabilityConnectorQuery query)
        {
            var availabilityRQ = BuildAvalabilityRequest(query);
            var availabilityResult = await _httpWrapper.Availability(query.ConnectionData, query.AuditRequests, query.Timeout, availabilityRQ);

            return availabilityResult;
        }

        public async Task<(ValuationRS? ValuationRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Valuation(ValuationConnectorQuery query)
        {
            var hotelValuationRq = BuildValuationRequest(query);
            var response = await _httpWrapper.Valuation(query.ConnectionData, query.Timeout, hotelValuationRq);
            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CreateBooking(BookingConnectorQuery query)
        {
            var hotelBookingRulesRQ = BuildBookingRequest(query);
            var response = await _httpWrapper.Booking(query.ConnectionData, hotelBookingRulesRQ);

            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> GetBookings(BookingsConnectorQuery query)
        {
            var getBookingRQ = BuildBookingGetRequest(query);
            var response = await _httpWrapper.GetBookings(query.ConnectionData, getBookingRQ);
            return response;
        }

        public async Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CancelBooking(BookingCancelConnectorQuery query)
        {
            var getBookingRQ = BuildCancelBookingRequest(query);
            var response = await _httpWrapper.CancelBooking(query.ConnectionData, getBookingRQ);

            return response;
        }

        private AvailabilityRq.AvailabilityRQ BuildAvalabilityRequest(AvailabilityConnectorQuery query)
        {
            //TODO: Implement this method
            var hotelAvailRQ = new AvailabilityRq.AvailabilityRQ()
            {
                rq = new NetStormingAvailabilityRQ()
                {
                    header = new RequestEnvelopeHeader()
                    {
                        actor = query.ConnectionData.Actor,
                        user = query.ConnectionData.User,
                        password = query.ConnectionData.Password,
                        version = ServiceConf.ApiVersion,
                        timestamp = DateTimeExtension.GetTimeStamp()
                    },
                    query = new AvailabilityEnvelopeQuery()
                    {
                        checkin = new envelopeQueryCheckin() { date = query.SearchCriteria.CheckInDate },
                        checkout = new envelopeQueryCheckout() { date = query.SearchCriteria.CheckOutDate },
                        type = "availability",
                        nationality = query.SearchCriteria.Nationality,
                        hotel = query.SearchCriteria.Accommodations.Select(x => new envelopeQueryHotel() { id = uint.Parse(x) }).ToArray(),
                        filters = new envelopeQueryFilters() { filter = "AVAILONLY" },
                        product = "hotel",
                        details = GetAvailRooms(query),
                    }
                }
            };

            if (query.Timeout > 0)
            {
                hotelAvailRQ.rq.query.timeout = query.Timeout / 1000;
            }
            ;

            return hotelAvailRQ;
        }


        private Models.Message.ValuationRQ.ValuationRQ BuildValuationRequest(ValuationConnectorQuery query)
        {
            // TODO: Implement this method
            var valuationRq = new Models.Message.ValuationRQ.ValuationRQ();
            return valuationRq;
        }



        private Models.Message.BookingRQ.BookRQ BuildBookingRequest(BookingConnectorQuery query)
        {
            // TODO: Implement this method
            var vc = query.ValuationCode;
            var bc = query.BookingCode;

            var result = new Models.Message.BookingRQ.BookRQ();

            return result;
        }

        private Models.Message.BookingRQ.BookRQ BuildBookingGetRequest(BookingsConnectorQuery query)
        {
            //TODO: Implement this method
            var result = new Models.Message.BookingRQ.BookRQ();

            return result;
        }
        private Models.Message.BookingRQ.BookRQ BuildCancelBookingRequest(BookingCancelConnectorQuery query)
        {
            //TODO: Implement this method
            var result = new Models.Message.BookingRQ.BookRQ();

            return result;
        }
        public envelopeQueryRoom[] GetAvailRooms(AvailabilityConnectorQuery rq)
        {
            var roomList = new List<envelopeQueryRoom>();
            foreach (var candidate in rq.SearchCriteria.RoomCandidates)
            {
                var adt = candidate.Pax.Where(x => x.Age > ServiceConf.MaxChildAge).Count();
                var chd = candidate.Pax.Where(x => x.Age > ServiceConf.MaxInfantAge && x.Age <= ServiceConf.MaxChildAge).Count();
                var inf = candidate.Pax.Where(x => x.Age <= ServiceConf.MaxInfantAge).Count();

                var room = new envelopeQueryRoom()
                {
                    occupancy = adt.ToString(),
                    required = 1
                };

                if (chd > 0)
                {
                    room.extrabed = true;
                    room.age = string.Join("-", candidate.Pax.Where(x => x.Age > ServiceConf.MaxInfantAge && x.Age <= ServiceConf.MaxChildAge).Select(x => x.Age.ToString()).ToArray());
                }
                ;

                if (inf > 0)
                {
                    room.cot = true;
                }
                ;

                roomList.Add(room);
            }

            var result = roomList.GroupBy(x => new { x.occupancy, x.cot, x.extrabed, x.age }).Select(y => new envelopeQueryRoom()
            {
                occupancy = y.Key.occupancy,
                required = (byte)y.Count(),
                age = y.Key.age,
                cot = y.Key.cot,
                extrabed = y.Key.extrabed,
                cotSpecified = y.Key.cot == true,
                extrabedSpecified = y.Key.extrabed == true
            });

            return result.ToArray();
        }

    }
}
