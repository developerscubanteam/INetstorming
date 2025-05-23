using Application.Dto.Common;

namespace Application.Dto.ValuationService
{
    public class ValuationQuery: BaseQuery
    {
        public int? Timeout { get; set; }
        public required string ValuationCode { get; set; }
    }
}
