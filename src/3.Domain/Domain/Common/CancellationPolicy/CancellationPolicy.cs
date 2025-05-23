namespace Domain.Common.CancellationPolicy
{
    public class CancellationPolicy
    {
        public string? Description { get; set; }
        public List<PenaltyRule>? PenaltyRules { get; set; }
    }
}
