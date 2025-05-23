using Domain.ValuationCode;
using Infrastructure.Connectivity.Queries.Base;

namespace Infrastructure.Connectivity.Queries
{
    public class ValuationConnectorQuery : ConnectorQueryBase
    {
        public int? Timeout { get; set; }
        public required ValuationCode ValuationCode { get; set; }
        public required VAConnectorAdvancedOptions AdvancedOptions { get; set; }
    }

    public class VAConnectorAdvancedOptions
    {
        public bool ShowBreakdownPrice { get; set; }
        public bool ShowHotelInfo { get; set; }
    }
}
