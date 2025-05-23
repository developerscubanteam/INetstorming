using Application.Dto.BookingCancelService;
using Domain.Booking;
using System.Threading.Tasks;

namespace Application.WorkFlow.Contracts
{
    public interface IBookingCancelService
    {
        Task<Booking> CancelBooking(BookingCancelQuery query);
    }
}
