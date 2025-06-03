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

        public static string GetValuationCode(StringBuilder vc, string propertyId, string agreement, decimal price,
            string searchNumber, List<RoomCandidates> RoomCandidates, string checkIn, string checkOut, string nationality,
            int? timeout)
        {

            var roomCandidates = new StringBuilder();
            foreach (var room in RoomCandidates)
            {
                roomCandidates.Append(room.RoomRefId).Append(RowFieldSeparator);
                roomCandidates.Append(room.RoomType).Append(RowFieldSeparator);
                roomCandidates.Append(room.Occupancy).Append(RowFieldSeparator);
                roomCandidates.Append(room.Edad).Append(RowFieldSeparator);
                roomCandidates.Append(room.Extrabed).Append(RowFieldSeparator);
                roomCandidates.Append(room.Cot).Append(RowFieldSeparator);
                foreach (var age in room.PaxesAge)
                {
                    roomCandidates.Append(age).Append(",");
                }
                roomCandidates.Length -= 1; //Remover la última ","
                roomCandidates.Append(RowSeparator);

            }
            roomCandidates.Length -= 2;//Remover el último "--"
            var stringRoomCandidates = roomCandidates.ToString();
            roomCandidates.Clear();

            vc.Append(propertyId).Append(FieldValuationSeparator);
            vc.Append(agreement).Append(FieldValuationSeparator);
            vc.Append(price).Append(FieldValuationSeparator);
            vc.Append(searchNumber).Append(FieldValuationSeparator);
            vc.Append(stringRoomCandidates).Append(FieldValuationSeparator);
            vc.Append(checkIn).Append(FieldValuationSeparator);
            vc.Append(checkOut).Append(FieldValuationSeparator);
            vc.Append(nationality).Append(FieldValuationSeparator);
            vc.Append(timeout);

            var valuationCode = vc.ToString();
            vc.Clear();

            return valuationCode;
        }
        public static ValuationCode DecodeValuationCode(string valuationCode)
        {
            var vcParams = valuationCode.Split(FieldValuationSeparator);
            var roomCandidates = vcParams[4].Split(RowSeparator);
            //var roomCandidatesList = new List<Tuple<int, IList<int>>>();
            var roomCandidatesList = new List<RoomCandidates>();
            foreach (var room in roomCandidates)
            {
                var roomFields = room.Split(RowFieldSeparator);
                var paxes = roomFields[6].Split(",");
                var paxesList = new List<byte>();
                foreach (var pax in paxes)
                {
                    paxesList.Add(byte.Parse(pax));
                }
                roomCandidatesList.Add(new RoomCandidates
                {
                    RoomRefId = int.Parse(roomFields[0]),
                    RoomType = roomFields[1],
                    Occupancy = byte.Parse(roomFields[2]),
                    Edad = roomFields[3],
                    Extrabed = bool.Parse(roomFields[4]),
                    Cot = bool.Parse(roomFields[5]),
                    PaxesAge = paxesList
                });
            }
            var vc = new ValuationCode()
            {
                PropertyId = vcParams[0],
                Agreement = vcParams[1],
                Price = decimal.Parse(vcParams[2]),
                SearchNumber = vcParams[3],
                RoomCandidates = roomCandidatesList,
                CheckIn = vcParams[5],
                CheckOut = vcParams[6],
                Nationality = vcParams[7],
                Timeout = decimal.TryParse(vcParams[8], out decimal timeout) ? timeout : null,
            };

            return vc;
        }

        public static string GetBookingCode(string vc, string searchNumber, string city)
        {
            var bc = new StringBuilder();
            bc.Append(vc).Append(FieldBookingSeparator);
            bc.Append(searchNumber).Append(FieldBookingSeparator);
            bc.Append(city);
            return bc.ToString();
        }

        public static BookingCode DecodeBookingCode(string bookingCode)
        {
            var bcParams = bookingCode.Split(FieldBookingSeparator);
            var bc = new BookingCode()
            {
                ValuationCode = bcParams[0],
                SearchNumber = bcParams[1],
                City = bcParams[2],
            };
            return bc;
        }
    }
}
