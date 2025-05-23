namespace Domain.Common
{
    public enum AuditDataType
    {
        Ok = 1,
        KO = 2,
        NoResults = 3,
        Unauthorized = 4,
        Timeout = 5,
        TooManyRequest = 6,
        BadRequest = 7
    }

    public class AuditData
    {
        public int NumberOfRequests { get; set; }
        public List<Request>? Requests { get; set; }
    }

    public class Request
    {
        public DateTime? TimeStamp { get; set; }
        public long? ProcessTime { get; set; }
        public string? RequestName { get; set; }
        public AuditDataType? Type { get; set; }
        public string? RQ { get; set; }
        public string? RS { get; set; }
    }
}
