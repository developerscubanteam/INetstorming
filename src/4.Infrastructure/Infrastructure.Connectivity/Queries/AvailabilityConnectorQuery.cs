using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Queries
{
    public class AvailabilityConnectorQuery : ConnectorQueryBase
    {
        public bool AuditRequests { get; set; }
        public int? Timeout { get; set; }
        public required SearchCriteria SearchCriteria { get; set; }
        public required AvConnectorAdvancedOptions AdvancedOptions { get; set; }
    }

    public class SearchCriteria
    {
        public string? Language { get; set; }
        public required DateTime CheckInDate { get; set; }
        public required DateTime CheckOutDate { get; set; }
        public string? Nationality { get; set; }
        public string? Market { get; set; }
        public required bool OnRequest { get; set; }
        public string? Currency { get; set; }
        public required IList<string> Accommodations { get; set; }
        public required IList<Room> RoomCandidates { get; set; }
    }

    public class Room
    {
        public int RoomRefId { get; set; }
        public required IList<Pax> Pax { get; set; }
    }

    public class AvConnectorAdvancedOptions
    {
        public bool ShowCancellationPolicies { get; set; }
        public bool ShowBreakdownPrice { get; set; }
        public bool ShowHotelInfo { get; set; }
    }

}
