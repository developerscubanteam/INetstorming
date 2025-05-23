namespace Application.Dto.Common
{
    public class BaseQuery
    {
        public Dictionary<string, List<string>>? Include { get; set; }
        public required Common.ExternalSupplier ExternalSupplier { get; set; }
    }
}
