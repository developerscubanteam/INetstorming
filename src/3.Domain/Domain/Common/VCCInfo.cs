namespace Domain.Common
{
    public class VCCInfo
    {
        public required string CardType { get; set; }
        public required string Number { get; set; }
        public required string CVC { get; set; }
        public required string ExpirationMonth { get; set; }
        public required string ExpirationYear { get; set; }
        public required string Holder { get; set; }
    }
}
