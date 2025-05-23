using Domain.Common;

namespace Domain.Booking
{
    public class Bookings
    {
        public List<Booking>? BookingList { get; set; }
        public List<Error.Error>? Errors { get; set; }
        public AuditData? AuditData { get; set; }

    }

}
