using Domain.Common.CancellationPolicy;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;

namespace Application.WorkFlow.Services
{
    class CancellationPolicyService
    {
        public static CancellationPolicy? GetCancellationPolicy(
             object? cancellationPolicy,
             decimal totalPrice,
             string currencyCode,
            DateTime checkInDate)
        {
            if (cancellationPolicy != null)
            {
                var result = new CancellationPolicy();

                // IMPOTRTANTE!!!!!
                //      1- Si no hay políticas de cancelación, debe devolverse NULL en lugar de un objeto vacío
                //      2- Nunca se debe devolver políticcas de cancelación con precio cero

                return result;
            }
            return null;
        }


    }
}
