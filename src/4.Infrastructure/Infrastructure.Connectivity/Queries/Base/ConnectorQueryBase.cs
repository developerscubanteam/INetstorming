namespace Infrastructure.Connectivity.Queries.Base
{

    public abstract class ConnectorQueryBase
    {        
        public required ConnectionData ConnectionData { get; set; }
    }
}
