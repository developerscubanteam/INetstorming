namespace Domain.ValuationCode
{
    public class ValuationCode
    {
        public required ICollection<RoomCandidates> RoomCandidates { get; set; }

        public required string PropertyId { get; set; }

        public required string CheckIn { get; set; }

        public required string CheckOut { get; set; }

        public required string Nationality { get; set; }

        public required string SearchNumber { get; set; }

        public required string Agreement { get; set; }

        public required decimal Price { get; set; }

        public decimal? Timeout { get; set; }

    }

    public class RoomCandidates
    {
        public int RoomRefId { get; set; }

        public IEnumerable<byte> PaxesAge { get; set; }

        public string RoomType { get; set; }

        public byte Occupancy { get; set; }

        public string Edad { get; set; }

        public bool Extrabed { get; set; }

        public bool Cot { get; set; }
    }
}
