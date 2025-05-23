using Application.Dto.Common;

namespace Application.Dto.AvailabilityService
{
    public class AvailabilityQuery : BaseQuery
    {
        public bool AuditRequests { get; set; }
        public int? Timeout { get; set; }
        public required SearchCriteria SearchCriteria { get; set; }
    }

    public class SearchCriteria
    {
        public string? Language { get; set; }
        public string? Nationality { get; set; }
        public string? Market { get; set; }
        public required DateTime CheckIn { get; set; }
        public required DateTime CheckOut { get; set; }
        public bool OnRequest { get; set; }
        public required IEnumerable<string> AccommodationCodes { get; set; }
        public required ICollection<Room> RoomCandidates { get; set; }
        public string? Currency { get; set; }
    }

    public class Room
    {
        public required int RoomRefId { get; set; }
        public required IEnumerable<byte> PaxesAge { get; set; }
    }
}
