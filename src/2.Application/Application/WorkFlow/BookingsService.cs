using Application.Dto.BookingsService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Booking;
using Domain.Error;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;

namespace Application.WorkFlow
{
    public class BookingsService : IBookingsService
    {
        private readonly IConnector _connector;

        public BookingsService(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<Bookings> GetBookings(BookingsQuery query)
        {
            return await CallConnector(query);
        }

        private async Task<Bookings> CallConnector(BookingsQuery query)
        {
            var bookings = new Bookings();
            try
            {
                var bookingRs = await _connector.GetBookings(ConvertToConnectoryQuery(query));
                BookingResponseService.ToListDto(query.Include, bookings, bookingRs);
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Provider);
                bookings.Errors = [error];
            }
            return bookings;
        }

        private BookingsConnectorQuery ConvertToConnectoryQuery(BookingsQuery query)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var connectionData = new ConnectionData()
            {
                Url = connection.Url,
                User = connection.User,
                Password = connection.Password,
                Actor = connection.Actor,
            };
            var connectorQuery = new BookingsConnectorQuery()
            {
                ConnectionData = connectionData,
                Locator = query.BookingId,
                ClientReference = query.ClientReference,
                Dates = query.Dates != null ? new Infrastructure.Connectivity.Queries.Dates()
                {
                    Type = query.Dates.Type == "Creation" ? DateType.Creation : DateType.Checkin
                ,
                    From = query.Dates.From,
                    To = query.Dates.To
                } : null,

            };

            return connectorQuery;
        }
    }
}
