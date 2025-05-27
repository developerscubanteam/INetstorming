using Domain.Booking;
using Domain.Common;
using Domain.Common.MinimumPrice;
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
                booking.Status = SetStatus(Response.BookingRS);
                booking.BookingId = "";


                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.Cancellocator.intance))
                    booking.CancelLocator = "";

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.HotelConformationCode.intance))
                    booking.HCN = "";

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.CheckInDate.intance))
                    booking.CheckIn = default;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.CheckOutDate.intance))
                    booking.CheckOut = default;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.HotelConformationCode.intance))
                    booking.ClientReference = default;

                if (IncludeService.CheckIfIsIncluded(include, keyInclude, BookingsK.Comments.intance))
                    booking.Comments = GetComments(default, default);

                if (IncludeService.CheckIfIsIncluded(include, Holder.intance, Holder.Empty.intance))
                    booking.Holder = GetHolder(default, default);

                if (IncludeService.CheckIfIsIncluded(include, Hotel.intance, Hotel.Empty.intance))
                    booking.Hotel = GetHotelInfo(include, default);

                if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Empty.intance))
                    booking.Mealplan = GetMealplanInfo(include, default);

                if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Empty.intance))
                    booking.Rooms = GetRooms(include, default, default);

                if (IncludeService.CheckIfIsIncluded(include, Prices.intance, Prices.Empty.intance))
                {
                    booking.Price = GetPrice(default);
                    booking.MinimumPrice = GetMinimumPrice(default);
                }

                if (IncludeService.CheckIfIsIncluded(include, Cancellationpolicy.intance, Cancellationpolicy.Empty.intance))
                    booking.CancellationPolicy = CancellationPolicyService.GetCancellationPolicy(default, default, default);

                if (IncludeService.CheckIfIsIncluded(include, Fees.intance, Fees.Empty.intance))
                    booking.Fees = GetFees(default);
            }

            return booking;
        }

        private static BookingHotel GetHotelInfo(Dictionary<string, List<string>>? include, object service)
        {
            //TODO: Fill hotel
            var hotel = new BookingHotel()
            {
                Code = default,
            };

            if (IncludeService.CheckIfIsIncluded(include, Hotel.intance, Hotel.Name.intance))
                hotel.Name = default;

            return hotel;
        }

        private static Mealplan GetMealplanInfo(Dictionary<string, List<string>>? include, object service)
        {
            // TODO: Fill mealplan
            var mealplan = new Mealplan()
            {
                Code = default,

            };

            if (IncludeService.CheckIfIsIncluded(include, Mealplans.intance, Mealplans.Name.intance))
                mealplan.Name = default;

            return mealplan;
        }
        private static List<Fee>? GetFees(object service)
        {
            // TODO: Fill fees
            return null;
        }

        private static Domain.Common.Price.Price GetPrice(List<object> prices)
        {
            // TODO: Fill price
            return null;

            //return PriceService.GetPrice(price.Currency, price.TotalFixAmounts.Gross.ToDecimal(), commissionable, commission, null);
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
            object pax)
        {
            // TODO: Fill pax
            var bkPax = new Domain.Common.Pax()
            {
                Name = default,
                Surname = default
            };

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Title.intance))
                bkPax.Title = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Address.intance))
                bkPax.Address = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Country.intance))
                bkPax.Country = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.City.intance))
                bkPax.City = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Age.intance))
                bkPax.Age = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Document.intance))
                bkPax.Document = null;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Email.intance))
                bkPax.Email = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Idpax.intance))
                bkPax.Id = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Phonenumber.intance))
                bkPax.PhoneNumber = default;

            if (IncludeService.CheckIfIsIncluded(include, key, Paxes.Postalcode.intance))
                bkPax.PostalCode = default;

            return bkPax;
        }

        private static List<BookingRoom>? GetRooms(Dictionary<string, List<string>>? include, object service,
            object paxes)
        {
            // TODO: Fill rooms

            return null;

            //var rooms = new List<BookingRoom>();
            //foreach (var room in service.HotelRooms)
            //{
            //    var roomType = "";
            //    if (room.RoomCategory != null)
            //        roomType = room.RoomCategory.Type;

            //    var occupancy = new List<Domain.Common.Pax>();
            //    foreach (var relpax in room.RelPaxes)
            //    {
            //        var pax = paxes.FirstOrDefault(x => x.IdPax == relpax.IdPax);
            //        if (pax != null)
            //            occupancy.Add(GetPax(include, Paxes.intance, pax));
            //    }

            //    var bookingRoom = new BookingRoom() { Code = roomType };

            //    if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Name.intance))
            //        bookingRoom.Name = room.Name;

            //    if (IncludeService.CheckIfIsIncluded(include, Rooms.intance, Rooms.Description.intance))
            //        bookingRoom.Description = room.Description;

            //    if (IncludeService.CheckIfIsIncluded(include, Paxes.intance, Paxes.Empty.intance))
            //        bookingRoom.Paxes = occupancy;

            //    rooms.Add(bookingRoom);
            //}
            //return rooms;

        }

        private static string GetComments(object reservation, object service)
        {

            // TODO: Fill comments
            var comments = "";

            return comments;
        }


        private static Status SetStatus(BookingRS BookingRS)
        {
            if (BookingRS == null)
                return Status.Error;

            var bookingStatus = "";

            var status = Status.Confirmed;
            switch (bookingStatus)
            {
                case "TAR":
                case "CON":
                case "PAG":
                    status = Status.Confirmed;
                    break;
                case "PDI":
                case "PRE":
                    status = Status.OnRequest;
                    break;
                case "CAC":
                case "CAN":
                    status = Status.Cancelled;
                    break;
            }

            return status;
        }

    }
}
