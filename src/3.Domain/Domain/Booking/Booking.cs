using Domain.Common;
using Domain.Common.CancellationPolicy;
using Domain.Common.MinimumPrice;
using Domain.Common.Price;

namespace Domain.Booking
{
    public enum Status
    {
        Confirmed = 1,
        OnRequest = 2,
        Error = 3,
        Cancelled = 4
    }

    public class Booking
    {
        public Status Status { get; set; }
        public string? Comments { get; set; }
        public string? Nationality { get; set; }
        public string? BookingId { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public string? ClientReference { get; set; }
        public string? CancelLocator { get; set; }
        public string? HCN { get; set; }        
        public VCCInfo? VCCInfo { get; set; }
        public Pax? Holder { get; set; }
        public BookingHotel? Hotel { get; set; }
        public Mealplan? Mealplan { get; set; }
        public IList<BookingRoom>? Rooms { get; set; }
        public Price? Price { get; set; }
        public MinimumPrice? MinimumPrice { get; set; }
        public CancellationPolicy? CancellationPolicy { get; set; }
        public IList<Fee>? Fees { get; set; }
        public List<Error.Error>? Errors { get; set; }
        public AuditData? AuditData { get; set; }
    }


    public class BookingHotel
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
    }



}
