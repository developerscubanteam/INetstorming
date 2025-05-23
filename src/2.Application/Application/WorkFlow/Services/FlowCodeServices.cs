using Domain.BookingCode;
using Domain.ValuationCode;
using System.Text;

namespace Application.WorkFlow.Services
{
    public static class FlowCodeServices
    {
        private const string FieldValuationSeparator = "^[";
        private const string FieldBookingSeparator = "^]";
        private const string RowSeparator = "__";
        private const string RowFieldSeparator = "~,";

        public static string GetValuationCode(StringBuilder vc,object data, ICollection<Application.Dto.AvailabilityService.Room> RoomCandidates = null)
        {

            var roomCandidates = new StringBuilder();
            foreach (var room in RoomCandidates)
            {
                roomCandidates.Append(room.RoomRefId).Append(RowFieldSeparator);
                foreach (var pax in room.PaxesAge)
                {
                    roomCandidates.Append(pax).Append(",");
                }
                roomCandidates.Append(RowSeparator);
            }
            var stringRoomCandidates = roomCandidates.ToString();

            //vc.Append(rateId).Append(FieldValuationSeparator);
            //vc.Append(roomId).Append(FieldValuationSeparator);
            //vc.Append(mealPlan).Append(FieldValuationSeparator);
            //vc.Append(accommodationCode).Append(FieldValuationSeparator);
            //vc.Append(stringRoomCandidates).Append(FieldValuationSeparator);
            //vc.Append(checkIn).Append(FieldValuationSeparator);
            //vc.Append(nights).Append(FieldValuationSeparator);            
            //vc.Append(language).Append(FieldValuationSeparator);
            //vc.Append(currency).Append(FieldValuationSeparator);
            //vc.Append(nationality);

            var valuationCode = vc.ToString();
            vc.Clear();

            return valuationCode;
        }
        public static ValuationCode DecodeValuationCode(string valuationCode)
        {
            var vcParams = valuationCode.Split(FieldValuationSeparator);
            var roomCandidates = vcParams[4].Split(RowSeparator);
            var roomCandidatesList = new List<Tuple<int, IList<int>>>();
            foreach (var room in roomCandidates)
            {
                var roomFields = room.Split(RowFieldSeparator);
                var paxes = roomFields[1].Split(",");
                var paxesList = new List<int>();
                foreach (var pax in paxes)
                {
                    paxesList.Add(int.Parse(pax));
                }
                roomCandidatesList.Add(new Tuple<int, IList<int>>(int.Parse(roomFields[0]), paxesList));
            }
            var vc = new ValuationCode()
            {
               RateId = vcParams[0],
               RoomId = vcParams[1],
               MealPlan = vcParams[2],
               AccommodationCode = vcParams[3],
               RoomCandidates = roomCandidatesList,
               CheckIn = vcParams[5],
               Nights = int.Parse(vcParams[6]),
               Language = vcParams[7],
               Currency = vcParams[8],
               Nationality = vcParams[9]
            };

            return vc;
        }

        public static string GetBookingCode(string vc, string bookingToken, decimal[]? amountBeforeTax, decimal[]? amountAfterTax)
        {
        //     public required string sessionID { get; set; }
        //public required string IdOperation { get; set; }
        //public required string AccommodationCode { get; set; }
        //public required string idDistributions { get; set; }

        var stringAmountBeforeTax = amountBeforeTax != null ? string.Join(",", amountBeforeTax) : "";
        var stringAmountAfterTax = amountAfterTax != null ? string.Join(",", amountAfterTax) : "";
        var bc = new StringBuilder();
            bc.Append(vc).Append(FieldBookingSeparator);
            bc.Append(bookingToken).Append(FieldBookingSeparator);
            bc.Append(DateTime.Now).Append(FieldBookingSeparator);
            bc.Append(stringAmountBeforeTax).Append(FieldBookingSeparator);
            bc.Append(stringAmountAfterTax);
            return bc.ToString();
        }

        public static BookingCode DecodeBookingCode(string bookingCode)
        {
            var bcParams = bookingCode.Split(FieldBookingSeparator);

            var bc = new BookingCode()
            {
               ValuationCode = bcParams[0],
               BookingToken = bcParams[1],
               amountBeforeTax = bcParams[3].Split(",").Select(decimal.Parse).ToArray(),
               amountAfterTax = bcParams[4].Split(",").Select(decimal.Parse).ToArray()
            };

            return bc;
        }
    }
}
