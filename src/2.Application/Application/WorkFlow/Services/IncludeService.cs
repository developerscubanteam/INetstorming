namespace Application.WorkFlow.Services
{
    static internal class IncludeService
    {
        public static bool CheckIfIsIncluded<TPadre, THijo>(Dictionary<string, List<string>>? include, TPadre parent, THijo? field)
        where TPadre : Parent
        where THijo : TPadre
        {
            if (include != null && parent.Clave !=null && include.ContainsKey(parent.Clave))
                if (field == null || field.Clave == null) //all fields
                    return true;
                else
                {
                    if (include[parent.Clave] == null)
                        return true; //all fields parent is true;
                    else
                        return include[parent.Clave].Any(x => x == field.Clave);
                }

            else
                return false;
        }
    }

    public abstract class Parent
    {
        public abstract string? Clave { get; }

        // ... otras propiedades y métodos comunes si es necesario
    }

    public class Accommodations : Parent
    {
        public static Accommodations intance = new Accommodations();
        public override string? Clave => "accommodations";
        public class Empty : Accommodations {
            public static new Empty intance = new Empty();
            public override string? Clave => null; 
        }
        public class Name : Accommodations {
            public static new Accommodations.Name intance = new Accommodations.Name();
            public override string Clave => "name"; 
        }       
    }

    public class Remarks : Parent 
    {        
        public static Remarks intance = new Remarks();
        public override string? Clave => "remarks";
        public class Empty : Remarks { 
            public static new Remarks.Empty intance = new Remarks.Empty();
            public override string? Clave => null; 
        }        
    }

    public class Fees : Parent
    {
        public static Fees intance = new Fees();
        public override string? Clave => "fees";
        public class Empty : Fees { 
            public static new Fees.Empty intance = new Fees.Empty();
            public override string? Clave => null;
        }      
    }

    public class RateConditions : Parent
    {
        public static RateConditions intance = new RateConditions();
        public override string? Clave => "rateconditions";
        public class Empty : RateConditions
        {
            public static new RateConditions.Empty intance = new RateConditions.Empty();
            public override string? Clave => null;
        }
    }
    public class Rooms : Parent
    {
        public static Rooms intance = new Rooms();
        public override string Clave => "rooms";
        public class Name : Rooms { 
            public static new Rooms.Name intance = new Rooms.Name();
            public override string Clave => "name"; 
        }
        public class Description : Rooms { 
            public static new Rooms.Description intance = new Rooms.Description();
            public override string Clave => "description"; 
        }
        public class Occupancy : Rooms
        {
            public static new Rooms.Occupancy intance = new Rooms.Occupancy();
            public override string Clave => "occupancy";
        }
        public class Empty : Rooms
        {
            public static new Rooms.Empty intance = new Rooms.Empty();
            public override string? Clave => null;
        }
    }
    public class Occupancy : Parent
    {
        public static Occupancy intance = new Occupancy();
        public override string? Clave => "occupancy";
        public class Empty : Occupancy { 
            public static new Occupancy.Empty intance = new Occupancy.Empty();
            public override string? Clave => null; 
        }
    }

    public class Cancellationpolicy : Parent
    {
        public static Cancellationpolicy intance = new Cancellationpolicy();
        public override string? Clave => "cancellationpolicy";
        public class Empty : Cancellationpolicy { 
            public static new Cancellationpolicy.Empty intance = new Cancellationpolicy.Empty();
            public override string? Clave => null; 
        }
    }

    public class Promotions : Parent
    {
        public static Promotions intance = new Promotions();
        public override string? Clave => "promotions";
        public class Empty : Promotions {
            public static new Promotions.Empty intance = new Promotions.Empty(); 
            public override string? Clave => null; 
        }
    }

    public class Mealplans : Parent
    {
        public static Mealplans intance = new Mealplans();
        public override string Clave => "mealplan";
        public class Name : Mealplans { 
            public static new Mealplans.Name intance = new Mealplans.Name();
            public override string Clave => "name"; 
        }
        public class Empty : Mealplans
        {
            public static new Mealplans.Empty intance = new Mealplans.Empty();
            public override string? Clave => null;
        }

    }

    public class Root : Parent
    {
        public static Root intance = new Root();
        public override string Clave => "root";
        public class Paymenttype : Root
        {
            public static new Root.Paymenttype intance = new Root.Paymenttype();
            public override string Clave => "paymenttype";
        }
        public class Code : Root
        {
            public static new Root.Code intance = new Root.Code();
            public override string Clave => "code";
        }
        public class Name : Root
        {
            public static new Root.Name intance = new Root.Name();
            public override string Clave => "name";
        }
        public class Remarks : Root
        {
            public static new Root.Remarks intance = new Root.Remarks();
            public override string Clave => "remarks";
        }
    }

    public class Prices : Parent
    {
        public static Prices intance = new Prices();
        public override string Clave => "price";        
        public class Empty : Prices
        {
            public static new Prices.Empty intance = new Prices.Empty();
            public override string? Clave => null;
        }
    }

    public class BookingsK : Parent
    {
        public static BookingsK intance = new BookingsK();
        public override string Clave => "bookings";
        public class Empty : BookingsK
        {
            public static new BookingsK.Empty intance = new BookingsK.Empty();
            public override string? Clave => null;
        }
        public class Cancellocator : BookingsK
        {
            public static new BookingsK.Cancellocator intance = new BookingsK.Cancellocator();
            public override string Clave => "cancellocator";
        }

        public class HotelConformationCode : BookingsK
        {
            public static new BookingsK.HotelConformationCode intance = new BookingsK.HotelConformationCode();
            public override string Clave => "hcn";
        }

        public class CheckInDate : BookingsK
        {
            public static new BookingsK.CheckInDate intance = new BookingsK.CheckInDate();
            public override string Clave => "checkin";
        }

        public class CheckOutDate : BookingsK
        {
            public static new BookingsK.CheckOutDate intance = new BookingsK.CheckOutDate();
            public override string Clave => "checkout";
        }
        public class ClientReference : BookingsK
        {
            public static new BookingsK.ClientReference intance = new BookingsK.ClientReference();
            public override string Clave => "clientreference";
        }
        public class Comments : BookingsK
        {
            public static new BookingsK.Comments intance = new BookingsK.Comments();
            public override string Clave => "comments";
        }
    }

    public class Holder : Parent
    {
        public static Holder intance = new Holder();
        public override string Clave => "holder";
        public class Empty : Holder
        {
            public static new Holder.Empty intance = new Holder.Empty();
            public override string? Clave => null;
        }
    }

    public class Hotel : Parent
    {
        public static Hotel intance = new Hotel();
        public override string Clave => "hotel";
        public class Empty : Hotel
        {
            public static new Hotel.Empty intance = new Hotel.Empty();
            public override string? Clave => null;
        }
        public class Name : Hotel
        {
            public static new Hotel.Name intance = new Hotel.Name();
            public override string Clave => "name";
        }
    }

    public class Paxes : Parent
    {
        public static Paxes intance = new Paxes();
        public override string Clave => "paxes";
        public class Empty : Paxes
        {
            public static new Paxes.Empty intance = new Paxes.Empty();
            public override string? Clave => null;
        }
        public class Title : Paxes
        {
            public static new Paxes.Title intance = new Paxes.Title();
            public override string Clave => "title";
        }
        public class Address : Paxes
        {
            public static new Paxes.Address intance = new Paxes.Address();
            public override string Clave => "address";
        }
        public class Country : Paxes
        {
            public static new Paxes.Country intance = new Paxes.Country();
            public override string Clave => "country";
        }
        //city, age, document, email, idpax, phonenumber, postalcode
        public class City : Paxes
        {
            public static new Paxes.City intance = new Paxes.City();
            public override string Clave => "city";
        }
        public class Age : Paxes
        {
            public static new Paxes.Age intance = new Paxes.Age();
            public override string Clave => "age";
        }
        public class Document : Paxes
        {
            public static new Paxes.Document intance = new Paxes.Document();
            public override string Clave => "document";
        }
        public class Email : Paxes
        {
            public static new Paxes.Email intance = new Paxes.Email();
            public override string Clave => "email";
        }
        public class Idpax : Paxes
        {
            public static new Paxes.Idpax intance = new Paxes.Idpax();
            public override string Clave => "idpax";
        }
        public class Phonenumber : Paxes
        {
            public static new Paxes.Phonenumber intance = new Paxes.Phonenumber();
            public override string Clave => "phonenumber";
        }
        public class Postalcode : Paxes
        {
            public static new Paxes.Postalcode intance = new Paxes.Postalcode();
            public override string Clave => "postalcode";
        }
    }


}
