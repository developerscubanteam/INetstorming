using Application.Dto.AvailabilityService;
using Domain.Availability;
using System.Threading.Tasks;

namespace Application.WorkFlow.Contracts
{
    public interface IAvailabilityService
    {
        Task<Availability> GetAvailability(AvailabilityQuery query);
    }
}
