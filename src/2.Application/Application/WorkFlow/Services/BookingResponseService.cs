using Domain.Booking;
using Domain.Common;
using Domain.Common.MinimumPrice;
using Infrastructure.Connectivity.Connector.Models;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;

namespace Application.WorkFlow.Services
{
    internal class BookingResponseService
    {
        public static Booking ToDto(Dictionary<string, List<string>>? include, Booking booking, (BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData) Response)
        {

            booking.AuditData = Response.AuditData;

            if (Response.Errors != null && Response.Errors.Any())
            {
                booking.Status = Status.Error;
                booking.Errors = Response.Errors;
                return booking;
            }

            AddBookingDetails(include, BookingsK.intance, booking, Response);

            return booking;
        }

        public static Bookings ToListDto(Dictionary<string, List<string>>? include, Bookings bookings, (BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData) Response)
        {

            bookings.AuditData = Response.AuditData;

            if (Response.Errors != null && Response.Errors.Any())
            {
                bookings.Errors = Response.Errors;
                return bookings;
            }

            var bookingList = new List<Booking>() { AddBookingDetails(include, BookingsK.intance, new Booking(), Response) };
            bookings.BookingList = bookingList;

            return bookings;
        }

        private static Booking AddBookingDetails(Dictionary<string, List<string>>? include, Parent keyInclude, Booking booking,
            (BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData) Response)
        {
            if (Response.BookingRS != null)
            {
                var response = Response.BookingRS.Booking.response;
                booking.Status = SetStatus(response.status.code);
                booking.BookingId = response.booking.code;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.Cancellocator.intance))
                    booking.CancelLocator = response.booking.code;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.HotelConformationCode.intance))
                    booking.HCN = response.reference.hcn;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.CheckInDate.intance))
                    booking.CheckIn = DateTime.TryParse(response.checkin.ToString(), out DateTime checkIn)
                        ? checkIn
                        : null;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.CheckOutDate.intance))
                    booking.CheckOut = DateTime.TryParse(response.checkout.ToString(), out DateTime checkOut)
                        ? checkOut
                        : null; ;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.HotelConformationCode.intance))
                    booking.ClientReference = response.reference.code;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.Comments.intance))
                    booking.Comments = GetComments(response.details);

                if (IncludeService.CheckIfIsIncluded(include, Holder.intance, Holder.Empty.intance))
                    booking.Holder = null; //GetHolder(default, default);

                if (IncludeService.CheckIfIsIncluded(include, Hotel.intance, Hotel.Empty.intance))
                    booking.Hotel = GetHotelInfo(include, response.hotel.code);

                if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Empty.intance))
                    booking.Mealplan = GetMealplanInfo(include, response.roombasis.meal);

                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Empty.intance))
                    booking.Rooms = GetRooms(include, response.details, response.hotel.room_type);

                if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
                {
                    booking.Price = GetPrice(response.details, response.currency.code);
                    booking.MinimumPrice = null;//GetMinimumPrice(default);
                }

                if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
                    booking.CancellationPolicy = null;//CancellationPolicyService.GetCancellationPolicy(default, default, default);

                if (IncludeService.CheckIfIsIncluded(include, Fees.intance, Fees.Empty.intance))
                    booking.Fees = null;//GetFees(default);
            }

            return booking;
        }

        private static BookingHotel GetHotelInfo(Dictionary<string, List<string>>? include, uint hotelCode)
        {
            //TODO: Fill hotel
            var hotel = new BookingHotel()
            {
                Code = hotelCode.ToString(),
            };

            if (IncludeService.CheckIfIsIncluded(include, Hotel.intance, Hotel.Name.intance))
                hotel.Name = null; //No viene

            return hotel;
        }

        private static Mealplan GetMealplanInfo(Dictionary<string, List<string>>? include, string code)
        {
            // TODO: Fill mealplan
            var mealplan = new Mealplan()
            {
                Code = code,

            };

            if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Name.intance))
                mealplan.Name = null; //No viene

            return mealplan;
        }
        private static List<Fee>? GetFees(object service)
        {
            // TODO: Fill fees
            return null;
        }

        private static Domain.Common.Price.Price GetPrice(envelopeResponseDetails details, string currency)
        {
            // TODO: Fill price           
            var price = details.room.Select(x => x.prices.roomprice.nett).Sum();
            return PriceService.GetPrice(currency, price, false, null, null);
        }

        private static MinimumPrice? GetMinimumPrice(List<object> prices)
        {
            // TODO: Fill minimum price
            //GetMinimumPrice(default,default)
            return null;
        }

        private static MinimumPrice? GetMinimumPrice(decimal? totalSellingPrice, string currency)
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

        private static Domain.Common.Pax? GetHolder(Dictionary<string, List<string>>? include,
            object paxes)
        {
            // TODO: Fill holder

            return null;
        }

        private static Domain.Common.Pax GetPax(Dictionary<string, List<string>>? include, Parent key,
            envelopeResponseRoomPax pax)
        {
            // TODO: Fill pax
            var bkPax = new Domain.Common.Pax()
            {
                Name = pax.initial,
                Surname = pax.surname
            };

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Title.intance))
                bkPax.Title = pax.title;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Address.intance))
                bkPax.Address = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Country.intance))
                bkPax.Country = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.City.intance))
                bkPax.City = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Age.intance))
                bkPax.Age = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Document.intance))
                bkPax.Document = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Email.intance))
                bkPax.Email = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Idpax.intance))
                bkPax.Id = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Phonenumber.intance))
                bkPax.PhoneNumber = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Postalcode.intance))
                bkPax.PostalCode = null;

            return bkPax;
        }

        private static List<BookingRoom>? GetRooms(Dictionary<string, List<string>>? include, envelopeResponseDetails details,
            string roomType)
        {
            // TODO: Fill rooms
            var rooms = new List<BookingRoom>();
            foreach (var room in details.room)
            {
                var occupancy = new List<Domain.Common.Pax>();
                foreach (var pax in room.pax)
                {
                    if (pax != null)
                        occupancy.Add(GetPax(include, Paxes.intance, pax));
                }

                var roomCodeName = roomType + "(" + room.type + ")";
                var bookingRoom = new BookingRoom()
                {
                    Code = roomCodeName
                };

                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Name.intance))
                    bookingRoom.Name = roomCodeName;
                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Description.intance))
                    bookingRoom.Description = null;
                if (IncludeService.CheckIfIsIncluded(include, Paxes.intance, Paxes.Empty.intance))
                    bookingRoom.Paxes = occupancy.Any() ? occupancy : null;

                rooms.Add(bookingRoom);
            }

            return rooms.Any() ? rooms : null;
        }

        private static string? GetComments(envelopeResponseDetails details)
        {
            // TODO: Fill comments
            if (details.remark != null && details.remark.Length > 0)
            {
                return string.Join(". ", details.remark.Select(x => x.Value));
            }
            else
            {
                return null;
            }
        }


        private static Status SetStatus(string? bookingStatus)
        {
            if (bookingStatus == null)
                return Status.Error;

            var status = Status.Confirmed;
            switch (bookingStatus)
            {
                case ServiceConf.Confirmed:
                    status = Status.Confirmed;
                    break;
                case ServiceConf.Canceled:
                    status = Status.Cancelled;
                    break;
                default:
                    status = Status.Error;
                    break;
            }

            return status;
        }

    }
}
