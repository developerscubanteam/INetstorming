using Application.Dto.Common;

namespace Application.Dto.BookingCreateService
{
    public enum ToleranceType
    {
        Percentage = 0,
        Amount = 1,
    }
    public class BookingCreateQuery : BaseQuery
    {
        public required string BookingCode { get; set; }
        public required string Locator { get; set; }
        public string? Remarks { get; set; }
        public VCC? VCCInfo { get; set; }
        public required Pax Holder { get; set; }
        public Tolerance? Tolerance { get; set; }    
        public required IEnumerable<Room> Rooms { get; set; }
    }

    public class Room
    {
        public required IList<Pax> Paxes { get; set; }
    }

    public class Pax
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? Document { get; set; }
        public string? PostalCode { get; set; }
        public int? Age { get; set; }
    }
    public class VCC
    {
        public required string CardCode { get; set; }
        public required string Number { get; set; }
        public required string Holder { get; set; }
        public required string CVC { get; set; }
        public required ValidityDate ValidityDate { get; set; }
    }

    public class Tolerance
    {
        public required ToleranceType Type { get; set; }
        public required decimal Value { get; set; }
    }

    public class ValidityDate
    {
        public required string Month { get; set; }
        public required string Year { get; set; }
    }
}
