using System.Threading.Tasks;

namespace Application.System.Contracts
{
    public interface IAuthorizationService
    {
        bool IsAuthorized(string apiKey);
    }
}
