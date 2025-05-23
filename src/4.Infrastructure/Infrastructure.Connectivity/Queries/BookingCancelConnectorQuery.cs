using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Queries
{
    public class BookingCancelConnectorQuery : ConnectorQueryBase
    {        
        public required string Locator { get; set; }
    }
}
