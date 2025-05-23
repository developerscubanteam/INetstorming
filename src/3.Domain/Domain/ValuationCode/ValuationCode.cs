namespace Domain.ValuationCode
{
    public class ValuationCode
    {
        public required string RateId { get; set; }
        public required string RoomId { get; set; }
        public required string MealPlan { get; set; }
        public required string CheckIn { get; set; }
        public required int Nights { get; set; }
        public required string AccommodationCode { get; set; }
        public required IList<Tuple<int,IList<int>>> RoomCandidates { get; set; } //RoomRef, Pax ages
        public string? Language { get; set; }
        public string? Currency { get; set; }
        public string? Nationality { get; set; }
    }
}
