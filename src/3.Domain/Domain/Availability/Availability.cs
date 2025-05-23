using Domain.Common;
using Domain.Common.CancellationPolicy;
using Domain.Common.MinimumPrice;
using Domain.Common.Price;

namespace Domain.Availability
{
    public enum StatusAvailability
    {
        Available = 1,
        OnRequest = 2
    }

    public enum RateConditionType
    {
        NonRefundable = 1,
        Package = 2,
        CanaryResident = 3,
        BalearicResident = 4,
        LargeFamily = 5,
        HoneyMoon = 6,
        Older55 = 7,
        Older60 = 8,
        Older65 = 9,
        PublicServant = 10,
        Unemployed = 11
    }

    public class Availability
    {
        public IList<Accommodation>? Accommodations { get; set; }
        public List<Error.Error>? Errors { get; set; }
        public AuditData? AuditData { get; set; }
    }

    public class Accommodation
    {
        public string? Code { get; set; }
        public string? Name { get; set; }

        public IList<Mealplan>? Mealplans { get; set; }
    }

    public class Mealplan
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public IList<Combination>? Combinations { get; set; }
    }

    public class Combination
    {
        public PaymentType PaymentType { get; set; }
        public bool? NonRefundable { get; set; }
        public StatusAvailability Status { get; set; }
        public string? ValuationCode { get; set; }
        public IList<Room>? Rooms { get; set; }
        public IList<string>? Remarks { get; set; }
        public IList<AdditionalService>? AdditionalServices { get; set; }
        public IList<RateConditionType>? RateConditions { get; set; }
        public IList<Promotion>? Promotions { get; set; }
        public Price? Price { get; set; }
        public MinimumPrice? MinimumPrice { get; set; }
        public CancellationPolicy? CancellationPolicy { get; set; }
        public IList<Fee>? Fees { get; set; }
    }

}
