using Application.Dto.BookingCreateService;
using Domain.Booking;
using System.Threading.Tasks;

namespace Application.WorkFlow.Contracts
{
    public interface IBookingCreateService
    {
        Task<Booking> CreateBooking(BookingCreateQuery query);
    }
}
