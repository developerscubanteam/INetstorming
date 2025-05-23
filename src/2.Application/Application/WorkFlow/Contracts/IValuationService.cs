using Application.Dto.ValuationService;
using Domain.Valuation;
using System.Threading.Tasks;

namespace Application.WorkFlow.Contracts
{
    public interface IValuationService
    {
        Task<Valuation> GetValuation(ValuationQuery query);
    }
}
