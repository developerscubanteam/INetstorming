namespace Infrastructure.Connectivity.Connector.Models
{
    public class ServiceConf
    {
        public const string ClientName = "GW";
        public const int TimeoutValuation = 180000;
        public const int TimeoutBooking = 180000;

        public const int MaxChildAge = 12;
        public const byte MaxInfantAge = 1;

        public const int MaxHotelsPerSearch = 200;

        public const string Name = "Netstorming";

        public const int AvailTimeout = 10000;
        public const int BookTimeout = 120000;

        public const string ApiVersion = "1.6.1";

        //BookingStatus
        public const string Pending = "pnd";
        public const string Confirmed = "cnf";
        public const string Canceled = "cxl";
        public const string Rejected = "rej";
    }
}