using Domain.Common;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;
using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Queries;

namespace Infrastructure.Connectivity.Contracts
{
    public interface IConnector
    {
        Task<(AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData)> Availability(AvailabilityConnectorQuery availabilityQuery);
        Task<(ValuationRS? ValuationRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Valuation(ValuationConnectorQuery valuationQuery);
        Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CreateBooking(BookingConnectorQuery query);
        Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CancelBooking(BookingCancelConnectorQuery cancelBookingConnectorQuery);
        Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> GetBookings(BookingsConnectorQuery query);
    }    
}
