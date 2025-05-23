using Application.Dto.Common;

namespace Application.Dto.BookingsService
{
    public class BookingsQuery : BaseQuery
    {
        public string? BookingId { get; set; }
        public string? ClientReference { get; set; }
        public Dates? Dates { get; set; }
    }
    public class Dates
    {
        public required string Type { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }
}
