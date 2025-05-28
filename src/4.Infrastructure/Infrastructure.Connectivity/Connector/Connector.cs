using Domain.Common;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRQ;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;
using Infrastructure.Connectivity.Connector.Models.Message.Common;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using System.Globalization;
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
            if (hotelBookingRulesRQ.Error != null)
            {
                var error = new Domain.Error.Error(
                    hotelBookingRulesRQ.Error.Code,
                    hotelBookingRulesRQ.Error.Message,
                    Domain.Error.ErrorType.Error, Domain.Error.CategoryErrorType.Hub);

                return (null, [error], new AuditData());
            }
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
            var vc = query.ValuationCode;
            envelopeQueryHotel[] hotel = { new envelopeQueryHotel() { id = uint.Parse(vc.PropertyId) } };
            var valuationRq = new Models.Message.ValuationRQ.ValuationRQ()
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
                        checkin = new envelopeQueryCheckin() { date = DateTime.ParseExact(vc.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) },
                        checkout = new envelopeQueryCheckout() { date = DateTime.ParseExact(vc.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) },
                        type = "availability",
                        nationality = vc.Nationality,
                        hotel = hotel,
                        product = "hotel",
                        details = GetRoomsFromValCode(vc),
                        search = new envelopeQuerySearch()
                        {
                            agreement = vc.Agreement,
                            number = vc.SearchNumber,
                            price = vc.Price,
                        },
                    }
                },
            };

            if (vc.Timeout != null && vc.Timeout > 0)
            {
                valuationRq.rq.query.timeout = vc.Timeout / 1000;
            }

            return valuationRq;
        }



        private Models.Message.BookingRQ.BookRQ BuildBookingRequest(BookingConnectorQuery query)
        {
            // TODO: Implement this method
            var vc = query.ValuationCode;
            var bc = query.BookingCode;
            var result = new Models.Message.BookingRQ.BookRQ();

            (BookingQueryRoom[] rooms, Infrastructure.Connectivity.Connector.Models.Message.Common.Error error) getRooms = GetBookingRooms(vc, query);
            if (getRooms.error != null)
            {
                result.Error = getRooms.error;
                return result;
            }

            BookingQueryHotel hotel = new BookingQueryHotel() { code = uint.Parse(vc.PropertyId), agreement = vc.Agreement };

            var rq = new NetstormingBookingRQ()
            {
                header = new RequestEnvelopeHeader()
                {
                    actor = query.ConnectionData.Actor,
                    user = query.ConnectionData.User,
                    password = query.ConnectionData.Password,
                    version = ServiceConf.ApiVersion,
                    timestamp = DateTimeExtension.GetTimeStamp()
                },
                query = new BookEnvelopeQuery()
                {
                    checkin = new envelopeQueryCheckin() { date = DateTime.ParseExact(vc.CheckIn, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) },
                    checkout = new envelopeQueryCheckout() { date = DateTime.ParseExact(vc.CheckOut, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None) },
                    type = "book",
                    nationality = vc.Nationality,
                    city = new envelopeQueryCity() { code = bc.City },
                    synchronous = new envelopeQuerySynchronous() { value = true },
                    hotel = hotel,
                    product = "hotel",
                    details = getRooms.Item1,
                    reference = new envelopeQueryReference() { code = query.ClientReference },
                    search = new envelopeQuerySearch() { number = bc.SearchNumber },
                    availonly = new envelopeQueryAvailonly() { value = true },
                }
            };
            result.rq = rq;
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

        public envelopeQueryRoom[] GetRoomsFromValCode(ValuationCode vc)
        {
            var result = vc.RoomCandidates.GroupBy(x => new { x.Occupancy, x.Edad, x.Extrabed, x.Cot, x.RoomType }).Select(y => new envelopeQueryRoom() // Tuple<roomtype,roomRefId,occupancy, edad, extrabed, cot>
            {
                required = (byte)y.Count(),
                age = y.Key.Edad,
                extrabed = y.Key.Extrabed,
                cot = y.Key.Cot,
                cotSpecified = y.Key.Cot == true,
                extrabedSpecified = y.Key.Extrabed == true,
                type = y.Key.RoomType
            });

            return result.ToArray();
        }

        public (BookingQueryRoom[] rooms, Error) GetBookingRooms(ValuationCode vc, BookingConnectorQuery query)
        {
            var result = new List<BookingQueryRoom>();
            var bcRoomCopy = vc.RoomCandidates.ToList();

            var alreadySetHolder = false;

            foreach (var item in query.Rooms)
            {
                var paxList = new List<envelopeQueryRoomPax>();
                // Se ordenan y concatenan las edades para comprar la ocupación de la habitación
                var matchRoom = vc.RoomCandidates.FirstOrDefault(x => string.Join("-", x.PaxesAge.OrderBy(z => z)) == string.Join("-", item.Paxes.Select(y => y.Age).OrderBy(p => p)));

                if (matchRoom == null)  // si no machean las edades con las registradas en el Valuation
                {
                    var error = new Error
                    {
                        Code = "400",
                        Message = "The room with passenger ages '" + string.Join("-", item.Paxes.Select(x => x.Age)) + "' provided in the Reservation request does not match match any of the rooms provided in the Availability search"
                    };
                    return (null, error);
                }
                ;

                var chdAges = matchRoom.Edad;
                var chdAgesList = string.IsNullOrEmpty(chdAges) ? null : chdAges.Split('-').Select(x => byte.Parse(x)).ToList();

                foreach (var pax in item.Paxes)
                {
                    var isHolder = (pax.Name == query.Holder.Name) && (pax.Surname == query.Holder.Surname) && !alreadySetHolder;

                    if (pax.Age > ServiceConf.MaxInfantAge) // No es necesario incluir el nombre del Infant en la RQ
                    {
                        var p = new envelopeQueryRoomPax()
                        {
                            name = pax.Name,
                            surname = pax.Surname,
                            title = "MR",
                            leader = isHolder,
                            leaderSpecified = isHolder,
                            age = (byte)((pax.Age > ServiceConf.MaxInfantAge && pax.Age <= ServiceConf.MaxChildAge) ? pax.Age : 0),
                            ageSpecified = (pax.Age > ServiceConf.MaxInfantAge && pax.Age <= ServiceConf.MaxChildAge),
                        };
                        alreadySetHolder = isHolder;

                        if (chdAgesList == null || !chdAgesList.Contains((byte)pax.Age))
                        {
                            p.age = 0;
                            p.ageSpecified = false;
                        }
                        else
                        {
                            chdAgesList.Remove((byte)pax.Age);
                        }

                        paxList.Add(p);
                    }
                }

                var room = new BookingQueryRoom()
                {
                    type = matchRoom.RoomType,
                    pax = paxList.ToArray(),
                    extrabed = matchRoom.Extrabed,
                    cot = matchRoom.Cot
                };

                result.Add(room);
                bcRoomCopy.Remove(matchRoom);
            }


            return (result.ToArray(), null);
        }


    }
}
