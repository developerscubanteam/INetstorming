using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;
using System.Xml.Serialization;

namespace Infrastructure.Connectivity.Connector.Models.Message.BookingRS
{
    public class BookingRS
    {
        public NetstormingBookingRS Booking { get; set; }
        public NetstormingBookingListRS BookingList { get; set; }
    }

    [XmlRoot("envelope")]
    public class NetstormingBookingListRS
    {
        [XmlElement("header")]
        public ResponseEnvelopeHeader Header { get; set; }

        [XmlElement("response")]
        public BookingListEnvelope Response { get; set; }

    }


    public class BookingListEnvelope
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlAttribute("product")]
        public string Product { get; set; }

        [XmlText]
        public string Value { get; set; }

        [XmlElement("services")]
        public BookingListServices Services { get; set; }

    }


    public class BookingListServices
    {
        [XmlAttribute("count")]
        public int Count { get; set; }

        [XmlElement("service")]
        public List<BookingService> List { get; set; }

    }


    public class BookingService
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("reference")]
        public string Reference { get; set; }

        [XmlAttribute("status")]
        public string Status { get; set; }

        [XmlAttribute("checkin")]
        public string CheckIn { get; set; }

        [XmlAttribute("checkout")]
        public string CheckOut { get; set; }

        [XmlAttribute("creationdate")]
        public string CreationDate { get; set; }

        [XmlAttribute("lastmodified")]
        public string LastModified { get; set; }

        [XmlAttribute("deadline")]
        public string Deadline { get; set; }

        [XmlAttribute("nett_price")]
        public decimal NettPrice { get; set; }

        [XmlAttribute("first_nett_price")]
        public decimal FirstNettPrice { get; set; }

        [XmlAttribute("currency")]
        public string Currency { get; set; }

        [XmlAttribute("hotel_id")]
        public string HotelId { get; set; }

        [XmlAttribute("hcn")]
        public string HCN { get; set; }

    }



    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "envelope")]
    public partial class NetstormingBookingRS
    {

        private ResponseEnvelopeHeader headerField;

        private BookingEnvelopeResponse responseField;

        /// <remarks/>
        public ResponseEnvelopeHeader header
        {
            get
            {
                return this.headerField;
            }
            set
            {
                this.headerField = value;
            }
        }

        /// <remarks/>
        public BookingEnvelopeResponse response
        {
            get
            {
                return this.responseField;
            }
            set
            {
                this.responseField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingEnvelopeResponse
    {

        private BookingResponseSupplier supplierField;

        private BookingResponseBooking bookingField;

        private BookingResponseInvoiced invoicedField;

        private BookingResponseStatus statusField;

        private EnvelopeResponseCheckin checkinField;

        private EnvelopeResponseCheckout checkoutField;

        private BookingResponseCity cityField;

        private BookingResponseHotel hotelField;

        private BookingResponseReference referenceField;

        private BookingResponseCurrency currencyField;

        private BookingResponseRoombasis roombasisField;

        private BookingResponseDeadline deadlineField;

        private envelopeResponseDetails detailsField;

        private string typeField;

        private string productField;

        private string valueField;

        /// <remarks/>
        public BookingResponseSupplier supplier
        {
            get
            {
                return this.supplierField;
            }
            set
            {
                this.supplierField = value;
            }
        }

        /// <remarks/>
        public BookingResponseBooking booking
        {
            get
            {
                return this.bookingField;
            }
            set
            {
                this.bookingField = value;
            }
        }

        /// <remarks/>
        public BookingResponseInvoiced invoiced
        {
            get
            {
                return this.invoicedField;
            }
            set
            {
                this.invoicedField = value;
            }
        }

        /// <remarks/>
        public BookingResponseStatus status
        {
            get
            {
                return this.statusField;
            }
            set
            {
                this.statusField = value;
            }
        }

        /// <remarks/>
        public EnvelopeResponseCheckin checkin
        {
            get
            {
                return this.checkinField;
            }
            set
            {
                this.checkinField = value;
            }
        }

        /// <remarks/>
        public EnvelopeResponseCheckout checkout
        {
            get
            {
                return this.checkoutField;
            }
            set
            {
                this.checkoutField = value;
            }
        }

        /// <remarks/>
        public BookingResponseCity city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        public BookingResponseHotel hotel
        {
            get
            {
                return this.hotelField;
            }
            set
            {
                this.hotelField = value;
            }
        }

        /// <remarks/>
        public BookingResponseReference reference
        {
            get
            {
                return this.referenceField;
            }
            set
            {
                this.referenceField = value;
            }
        }

        /// <remarks/>
        public BookingResponseCurrency currency
        {
            get
            {
                return this.currencyField;
            }
            set
            {
                this.currencyField = value;
            }
        }

        /// <remarks/>
        public BookingResponseRoombasis roombasis
        {
            get
            {
                return this.roombasisField;
            }
            set
            {
                this.roombasisField = value;
            }
        }

        /// <remarks/>
        public BookingResponseDeadline deadline
        {
            get
            {
                return this.deadlineField;
            }
            set
            {
                this.deadlineField = value;
            }
        }

        /// <remarks/>
        public envelopeResponseDetails details
        {
            get
            {
                return this.detailsField;
            }
            set
            {
                this.detailsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string product
        {
            get
            {
                return this.productField;
            }
            set
            {
                this.productField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseSupplier
    {

        private string codeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseBooking
    {

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseInvoiced
    {

        private string byField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string by
        {
            get
            {
                return this.byField;
            }
            set
            {
                this.byField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseStatus
    {

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseCity
    {

        private string codeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseHotel
    {

        private uint codeField;

        private string agreementField;

        private string ctypeField;

        private string c_typeField;

        private string cityField;

        private bool is_dynamicField;

        private string room_typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agreement
        {
            get
            {
                return this.agreementField;
            }
            set
            {
                this.agreementField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string ctype
        {
            get
            {
                return this.ctypeField;
            }
            set
            {
                this.ctypeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string c_type
        {
            get
            {
                return this.c_typeField;
            }
            set
            {
                this.c_typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string city
        {
            get
            {
                return this.cityField;
            }
            set
            {
                this.cityField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool is_dynamic
        {
            get
            {
                return this.is_dynamicField;
            }
            set
            {
                this.is_dynamicField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string room_type
        {
            get
            {
                return this.room_typeField;
            }
            set
            {
                this.room_typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseReference
    {

        private string codeField;

        private string hcnField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string hcn
        {
            get
            {
                return this.hcnField;
            }
            set
            {
                this.hcnField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseCurrency
    {

        private string codeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseRoombasis
    {

        private string mealField;

        private string breakfastField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string meal
        {
            get
            {
                return this.mealField;
            }
            set
            {
                this.mealField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string breakfast
        {
            get
            {
                return this.breakfastField;
            }
            set
            {
                this.breakfastField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseDeadline
    {

        private System.DateTime dateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class BookingResponseRoom
    {

        private envelopeResponseRoomPax[] paxField;

        private envelopeResponseRoomPrices pricesField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("pax")]
        public envelopeResponseRoomPax[] pax
        {
            get
            {
                return this.paxField;
            }
            set
            {
                this.paxField = value;
            }
        }

        /// <remarks/>
        public envelopeResponseRoomPrices prices
        {
            get
            {
                return this.pricesField;
            }
            set
            {
                this.pricesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseRoomPax
    {

        private string titleField;

        private string initialField;

        private string surnameField;

        private bool leaderField;

        private bool leaderFieldSpecified;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string title
        {
            get
            {
                return this.titleField;
            }
            set
            {
                this.titleField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string initial
        {
            get
            {
                return this.initialField;
            }
            set
            {
                this.initialField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string surname
        {
            get
            {
                return this.surnameField;
            }
            set
            {
                this.surnameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool leader
        {
            get
            {
                return this.leaderField;
            }
            set
            {
                this.leaderField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool leaderSpecified
        {
            get
            {
                return this.leaderFieldSpecified;
            }
            set
            {
                this.leaderFieldSpecified = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseRoomPrices
    {

        private envelopeResponseRoomPricesRoomprice roompriceField;

        /// <remarks/>
        public envelopeResponseRoomPricesRoomprice roomprice
        {
            get
            {
                return this.roompriceField;
            }
            set
            {
                this.roompriceField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseRoomPricesRoomprice
    {

        private envelopeResponseRoomPricesRoompriceBydate[] bydateField;

        private decimal nettField;

        private decimal grossField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("bydate")]
        public envelopeResponseRoomPricesRoompriceBydate[] bydate
        {
            get
            {
                return this.bydateField;
            }
            set
            {
                this.bydateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal nett
        {
            get
            {
                return this.nettField;
            }
            set
            {
                this.nettField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal gross
        {
            get
            {
                return this.grossField;
            }
            set
            {
                this.grossField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseRoomPricesRoompriceBydate
    {

        private System.DateTime fromField;

        private decimal nettField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "date")]
        public System.DateTime from
        {
            get
            {
                return this.fromField;
            }
            set
            {
                this.fromField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal nett
        {
            get
            {
                return this.nettField;
            }
            set
            {
                this.nettField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseDetails
    {

        private BookingResponseRoom[] roomField;

        private envelopeResponseDetailsRemark[] remarkField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("room")]
        public BookingResponseRoom[] room
        {
            get
            {
                return this.roomField;
            }
            set
            {
                this.roomField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("remark")]
        public envelopeResponseDetailsRemark[] remark
        {
            get
            {
                return this.remarkField;
            }
            set
            {
                this.remarkField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseDetailsRemark
    {

        private string codeField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }
    }


}