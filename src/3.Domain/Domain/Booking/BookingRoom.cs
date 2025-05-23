using Domain.Common;

namespace Domain.Booking
{
    public class BookingRoom
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<Pax>? Paxes { get; set; }
    }
}
