namespace Domain.Availability
{
    public enum ServiceType
    {
        SkiPass = 1,
        Lessons = 2,
        Meals = 3,
        Equipment = 4,
        Ticket = 5,
        Transfers = 6,
        Gala = 7,
        Activity = 8,
        Unknow = 9
    }
    public enum ServiceDurationType
    {
        Range = 1,
        Open = 2

    }
    public enum ServiceUnitType
    {
        Day = 1,
        Hour = 2
    }

    public class AdditionalService
    {
        public string? Code { get; set; }
        public string? Descritpion { get; set; }
        public ServiceType ServiceType { get; set; }
        public ServiceDurationType DurationType { get; set; }
        public int Quantity { get; set; }
        public ServiceUnitType UnitType { get; set; }
        public RangeDates? RangeDates { get; set; }
    }
}
