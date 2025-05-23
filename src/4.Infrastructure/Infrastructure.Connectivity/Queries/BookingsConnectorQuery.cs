using Application.Dto.Common;
using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Queries
{
    public class BookingsConnectorQuery : ConnectorQueryBase
    {
        public string? Locator { get; set; }
        public string? ClientReference { get; set; }
        public Dates? Dates { get; set; }
    }

    public class Dates
    {
        public required DateType Type { get; set; }
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
    }

    public enum DateType
    {
        Creation,
        Checkin
    }


}
