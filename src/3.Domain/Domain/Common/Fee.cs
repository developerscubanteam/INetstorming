namespace Domain.Common
{
    public class Fee
    {
        public required string Description { get; set; }
        public string? Currency { get; set; }
        public bool IncludedInPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal? Percent { get; set; }
    }
}
