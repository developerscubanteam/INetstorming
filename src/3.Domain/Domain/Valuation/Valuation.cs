using Domain.Common;
using Domain.Common.CancellationPolicy;
using Domain.Common.MinimumPrice;
using Domain.Common.Price;
using System.Security.Cryptography.X509Certificates;

namespace Domain.Valuation
{
    public enum StatusValuation
    {
        Available = 1,
        OnRequest = 2
    }

    public class Valuation
    {
        public StatusValuation Status { get; set; }
        public string? BookingCode { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public IEnumerable<string>? Remarks { get; set; }
        public CancellationPolicy? CancellationPolicy { get; set; }
        public Mealplan? Mealplan { get; set; }
        public Price? Price { get; set; }
        public IEnumerable<Fee>? Fees { get; set; }
        public IEnumerable<Room>? Rooms { get; set; }
        public MinimumPrice? MinimumPrice { get; set; }
        public PaymentType? PaymentType { get; set; }
        public List<Promotion>? Promotions { get; set; }
        public DateTime? Expiration { get; set; }
        public List<Error.Error>? Errors { get; set; }
        public AuditData? AuditData { get; set; }
    }
}
