namespace Domain.Common
{
    public class Pax
    {
        public int? Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public string? Document { get; set; }
        public int? Age { get; set; }
        public string? Title { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? PostalCode { get; set; }
    }
}
