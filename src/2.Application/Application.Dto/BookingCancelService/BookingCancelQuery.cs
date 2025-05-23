using Application.Dto.Common;

namespace Application.Dto.BookingCancelService
{
    public class BookingCancelQuery: BaseQuery
    {
        public required string BookingId { get; set; }
    }

}
