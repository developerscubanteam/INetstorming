using Newtonsoft.Json;

namespace Infrastructure.Connectivity.Connector.Models.Message.ErrorRS
{

    public class ErrorRS
    {
        public required string errorCode { get; set; }
        public required string errorMessage { get; set; }
        public string ? supplierErrorCode { get; set; }
    }

}
