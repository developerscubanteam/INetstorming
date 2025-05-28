using Application.Dto.BookingCreateService;
using Application.WorkFlow.Contracts;
using Application.WorkFlow.Services;
using Domain.Booking;
using Domain.Common;
using Domain.Error;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Queries;
using Infrastructure.Connectivity.Queries.Base;

namespace Application.WorkFlow
{
    public class BookingCreateService : IBookingCreateService
    {
        private readonly IConnector _connector;

        public BookingCreateService(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<Booking> CreateBooking(BookingCreateQuery query)
        {
            return await CallConnector(query);
        }

        private async Task<Booking> CallConnector(BookingCreateQuery query)
        {
            var bc = FlowCodeServices.DecodeBookingCode(query.BookingCode);
            var booking = new Booking();
            try
            {

                var bookingRS = await _connector.CreateBooking(ConvertToConnectoryQuery(query, bc));
                BookingResponseService.ToDto(query.Include, booking, bookingRS);
            }
            catch (Exception ex)
            {
                var error = new Domain.Error.Error("UncontrolledException", ex.GetFullMessage(), ErrorType.Error, CategoryErrorType.Provider);
                booking.Errors = [error];

            }
            return booking;
        }

        private ValuationConnectorQuery ConvertToValuationConnectoryQuery(BookingCreateQuery query, ValuationCode valuationCode)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var connectionData = new ConnectionData()
            {
                Url = connection.Url,
                User = connection.User,
                Password = connection.Password,
                Actor = connection.Actor,
            };

            var connectorQuery = new ValuationConnectorQuery()
            {
                AdvancedOptions = new VAConnectorAdvancedOptions()
                {
                    ShowBreakdownPrice = false
                },
                ConnectionData = connectionData,
                ValuationCode = valuationCode
            };

            return connectorQuery;
        }

        private BookingConnectorQuery ConvertToConnectoryQuery(BookingCreateQuery query, Domain.BookingCode.BookingCode bookingCode)
        {
            var connection = query.ExternalSupplier.GetConnection();
            var connectionData = new ConnectionData()
            {
                Url = connection.Url,
                User = connection.User,
                Password = connection.Password,
                Actor = connection.Actor,
            };

            var vc = FlowCodeServices.DecodeValuationCode(bookingCode.ValuationCode);
            var connectorQuery = new BookingConnectorQuery()
            {
                ConnectionData = connectionData,
                BookingCode = bookingCode,
                ValuationCode = vc,
                JPBookingCode = "",
                ClientReference = query.Locator,
                Remark = GetRemark(query),
                BillingInformation = GetVCCData(query),
                Rooms = query.Rooms.Select(x => new Infrastructure.Connectivity.Queries.BookingRoom()
                {
                    Paxes = GetBookingPaxes(x)
                }).ToList(),
                Holder = GetHolder(query.Holder),
                Tolerance = GetTolerance(query.Tolerance)
            };
            return connectorQuery;
        }

        private BillingInformation? GetVCCData(BookingCreateQuery query)
        {
            if (query.VCCInfo == null)
                return null;

            return new BillingInformation
            {
                CardType = query.VCCInfo.CardCode,
                Number = query.VCCInfo.Number,
                CVC = query.VCCInfo.CVC,
                ExpirationMonth = query.VCCInfo.ValidityDate.Month,
                ExpirationYear = query.VCCInfo.ValidityDate.Year,
                Holder = query.VCCInfo.Holder
            };
        }
        private BookingPax GetHolder(Dto.BookingCreateService.Pax holder)
        {
            return new BookingPax
            {
                IdPax = holder.Id.ToString(),
                Name = holder.Name,
                Surname = holder.Surname,
                Address = holder.Address,
                City = holder.City,
                Country = holder.Country,
                Title = holder.Title,
                Document = holder.Document,
                Email = holder.Email,
                PhoneNumber = holder.PhoneNumber,
                PostalCode = holder.PostalCode,
                Age = holder.Age
            };
        }

        private Infrastructure.Connectivity.Queries.Tolerance? GetTolerance(Dto.BookingCreateService.Tolerance? tolerance)
        {
            if (tolerance != null)
            {
                return new Infrastructure.Connectivity.Queries.Tolerance()
                {
                    Value = tolerance.Value,
                    Type = tolerance.Type == Dto.BookingCreateService.ToleranceType.Amount ? Infrastructure.Connectivity.Queries.ToleranceType.Amount
                                                                                           : Infrastructure.Connectivity.Queries.ToleranceType.Percentage
                };
            }
            return null;
        }

        private IList<BookingPax> GetBookingPaxes(Dto.BookingCreateService.Room room)
        {
            var paxes = new List<BookingPax>();

            string? document = null;
            foreach (var pax in room.Paxes)
            {
                paxes.Add(new BookingPax
                {
                    IdPax = pax.Id.ToString(),
                    Name = pax.Name,
                    Surname = pax.Surname,
                    Address = pax.Address,
                    Title = pax.Title,
                    City = pax.City,
                    Country = pax.Country,
                    Document = document,
                    Email = pax.Email,
                    PhoneNumber = pax.PhoneNumber,
                    PostalCode = pax.PostalCode,
                    Age = pax.Age,
                    Type = pax.Age >= 18 ? "AD" : "CH"
                });
            }

            return paxes;
        }

        private string? GetRemark(BookingCreateQuery query)
        {
            var remarks = query.Remarks;
            if (!string.IsNullOrWhiteSpace(remarks) && remarks.Length > 2000)
                remarks = remarks.Substring(0, 2000);

            return remarks;
        }

        private Booking ThrowErrorRegenerationBookingCode(List<Domain.Error.Error>? errors, AuditData? auditData)
        {
            var booking = new Booking
            {
                Errors = errors
            };

            if (auditData != null)
                booking.AuditData = auditData;

            return booking;
        }

    }
}
