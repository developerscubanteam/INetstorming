using Application.Dto.BookingCancelService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Booking;
using Domain.Error;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;

namespace Application.WorkFlow
{
    public class BookingCancelService : IBookingCancelService
    {
        private readonly IConnector _connector;

        public BookingCancelService(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<Booking> CancelBooking(BookingCancelQuery query)
        {
            return await CallConnector(query);
        }

        private async Task<Booking> CallConnector(BookingCancelQuery query)
        {
            var booking = new Booking();
            try
            {
                var bookingRS = await _connector.CancelBooking(ConvertToConnectoryQuery(query));
                BookingResponseService.ToDto(query.Include, booking, bookingRS);
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Provider);
                booking.Errors = [error];
            }
            return booking;
        }

        private BookingCancelConnectorQuery ConvertToConnectoryQuery(BookingCancelQuery query)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var connectionData = new ConnectionData()
            {
                Url = connection.Url,
                User = connection.User,
                Password = connection.Password,
                Actor = connection.Actor,
            };

            var connectorQuery = new BookingCancelConnectorQuery()
            {
                ConnectionData = connectionData,
                Locator = query.BookingId
            };

            return connectorQuery;
        }

    }
}
