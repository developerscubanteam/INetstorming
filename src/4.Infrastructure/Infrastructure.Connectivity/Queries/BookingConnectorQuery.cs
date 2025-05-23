using Application.Dto.BookingCreateService;
using Domain.BookingCode;
using Domain.ValuationCode;
using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Queries
{
    public enum ToleranceType
    {
        Percentage = 0,
        Amount = 1,
    }

    public class BookingConnectorQuery : ConnectorQueryBase
    {
        public required BookingCode BookingCode { get; set; }
        public required ValuationCode ValuationCode { get; set; }   
        public required string JPBookingCode { get; set; }
        public required string ClientReference { get; set; }
        public required BookingPax Holder { get; set; }
        public required IList<BookingRoom> Rooms { get; set; }
        public Tolerance? Tolerance { get; set; }
        public string? Remark { get; set; }
        public BillingInformation? BillingInformation { get; set; }
    }

    public class BookingRoom
    {
        public required IList<BookingPax> Paxes { get; set; }
    }

    public class BookingPax
    {
        public required string IdPax { get; set; }
        public string? Type { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Title { get; set; }
        public string? Document { get; set; }
        public int? Age { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
    }
    public class BillingInformation
    {
        public required string CardType { get; set; }
        public required string Number { get; set; }
        public required string CVC { get; set; }
        public required string ExpirationMonth { get; set; }
        public required string ExpirationYear { get; set; }
        public required string Holder { get; set; }
    }

    public class Tolerance
    {
        public required ToleranceType Type { get; set; }
        public required decimal Value { get; set; }
    }
}
