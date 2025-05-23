using Domain.Common;

namespace Domain.BookingCode
{
    public class BookingCode
    {
        public DateTime Expirationdate { get; set; }
        public required string ValuationCode { get; set; }
        public required string BookingToken { get; set; }
        public decimal[]? amountBeforeTax { get; set; }
        public decimal[]? amountAfterTax { get; set; }
    }

    public class Fee
    {
        public Daterange dateRange { get; set; }
        public Fee1 fee { get; set; }
    }

    public class Daterange
    {
        public string startDate { get; set; }
        public string endDate { get; set; }
    }

    public class Fee1
    {
        public string name { get; set; }
        public string type { get; set; }
        public int amount { get; set; }
        public string amountType { get; set; }
        public string chargeType { get; set; }
        public string paymentType { get; set; }
        public int effectivePerson { get; set; }
    }
}
