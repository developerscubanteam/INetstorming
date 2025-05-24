namespace Application.Dto.Common
{
    public enum RateType { Net = 1, Commissionable = 2 }
    public class ExternalSupplier
    {
        public required string Code { get; set; }
        public required Dictionary<string, string> Connection { get; set; }
        public Dictionary<string, string>? Params { get; set; }

        public ConnectionData GetConnection()
        {
            var connection = new ConnectionData()
            {
                Url = Connection["Url"],
                User = Connection["User"],
                Password = Connection["Password"],
                Actor = Connection["Actor"]
            };
            return connection;
        }

    }
}
