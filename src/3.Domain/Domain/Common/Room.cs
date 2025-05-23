namespace Domain.Common
{
    public class Room
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public required int RoomRefId { get; set; }
        public Price.Price? Price { get; set; }
        public RoomOccupancy? Occupancy { get; set; }
        public List<DailyPriceDto>? DailyPrices { get; set; }
        public List<BedDto>? Beds { get; set; }
        public List<Feature>? Features { get; set; }
        public List<Promotion>? Promotions { get; set; }
    }

    public class RoomOccupancy
    {
        public int? Occupancy { get; set; }
        public int? MaxOccupancy { get; set; }
        public int? MinOccupancy { get; set; }
        public int? AvailableRooms { get; set; }
        public int? Adults { get; set; }
        public int? MaxAdults { get; set; }
        public int? MinAdults { get; set; }
        public int? Children { get; set; }
        public int? MaxChildren { get; set; }
    }

    public class DailyPriceDto
    {
        public required DateTime From { get; set; }
        public required DateTime To { get; set; }
        public required Price.Price Price { get; set; }
    }

    public class BedDto
    {
        public required string Type { get; set; }
        public required int Quantity { get; set; }
    }

}
