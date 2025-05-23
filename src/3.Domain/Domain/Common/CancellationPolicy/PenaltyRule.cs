
using System.Text.Json.Serialization;

namespace Domain.Common.CancellationPolicy
{
    public enum PolicyRule 
    { 
        AmountRule = 1,
        NightRule = 2,
        NonRefundable = 3,
        NoShowRule = 4,
        PercentageRule = 5
    }

    public class PenaltyRule
    {
        public required PolicyRule Type { get; set; }
        public DateTime DateFrom { get; set; }
        public short? Nights { get; set; }
        public decimal? Percentage { get; set; }
        public Domain.Common.Price.Price? Price { get; set; }
        public string? CurrencyCode { get; set; }

    }
}
