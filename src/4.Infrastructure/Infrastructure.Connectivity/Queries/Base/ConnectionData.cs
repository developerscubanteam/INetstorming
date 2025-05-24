namespace Infrastructure.Connectivity.Queries.Base
{
    public class ConnectionData
    {
        public required string Url { get; set; }
        public required string User { get; set; }
        public required string Password { get; set; }
        public required string Actor { get; set; }
    }
}
