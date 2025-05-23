namespace Domain.Common.Price
{
    public enum PurchaseCostType
    {
        Nett = 1,
        Commissionable
    }
    public class Purchase
    {      
        public Cost? Cost { get; set; }     
        public PurchaseCommission? Commission { get; set; }
        public required Decimal Gross { get; set; }
        public required Decimal Net { get; set; }
        public PurchaseCostType CostType { get; set; }
        public required string CurrencyCode { get; set; }
    }
}
