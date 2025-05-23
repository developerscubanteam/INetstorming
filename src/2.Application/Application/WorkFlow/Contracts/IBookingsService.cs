using Application.Dto.BookingsService;
using Domain.Booking;

namespace Application.WorkFlow.Contracts
{
    public interface IBookingsService
    {
        Task<Bookings> GetBookings(BookingsQuery query);
    }
}
