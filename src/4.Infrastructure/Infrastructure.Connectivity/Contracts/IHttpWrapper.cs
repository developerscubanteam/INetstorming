using Domain.Common;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using Infrastructure.Connectivity.Connector.Models.Message.BookingRS;

using Infrastructure.Connectivity.Connector.Models.Message.ValuationRS;
using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Contracts
{
    public interface IHttpWrapper
    {
        Task<(AvailabilityRS? AvailabilityRs, List<Domain.Error.Error>? Errors, AuditData AuditData)> Availability(ConnectionData connectionData, bool auditRequests, int? timeout, object query);        
        Task<(ValuationRS? ValuationRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Valuation(ConnectionData connectionData, int? timeout, object query);
        Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> Booking(ConnectionData connectionData, object query);
        Task<(BookingRS? BookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> CancelBooking(ConnectionData connectionData, object query);
        Task<(BookingRS? GetBookingRS, List<Domain.Error.Error>? Errors, AuditData AuditData)> GetBookings(ConnectionData connectionData, object query);        
    }

}
