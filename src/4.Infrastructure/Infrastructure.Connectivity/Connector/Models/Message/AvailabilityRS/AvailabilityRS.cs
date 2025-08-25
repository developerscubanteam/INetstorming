namespace Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS
{

    public class AvailabilityRS
    {
        public NetStormingAvailabilityRS rs { get; set; }
    }
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "envelope")]
    public partial class NetStormingAvailabilityRS
    {

        private ResponseEnvelopeHeader headerField;

        private AvailEnvelopeResponse responseField;

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
        public AvailEnvelopeResponse response
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
    public partial class ResponseEnvelopeHeader
    {

        private envelopeHeaderVersion versionField;

        private ulong timestampField;

        /// <remarks/>
        public envelopeHeaderVersion version
        {
            get
            {
                return this.versionField;
            }
            set
            {
                this.versionField = value;
            }
        }

        /// <remarks/>
        public ulong timestamp
        {
            get
            {
                return this.timestampField;
            }
            set
            {
                this.timestampField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeHeaderVersion
    {

        private ushort portField;

        private string hostField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort port
        {
            get
            {
                return this.portField;
            }
            set
            {
                this.portField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string host
        {
            get
            {
                return this.hostField;
            }
            set
            {
                this.hostField = value;
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
    public partial class AvailEnvelopeResponse
    {

        private AvailResponseSearch searchField;

        private envelopeResponseNights nightsField;

        private EnvelopeResponseCheckin checkinField;

        private EnvelopeResponseCheckout checkoutField;

        private envelopeResponseEvaluate evaluateField;

        private AvailResponseHotels hotelsField;

        private string typeField;

        private string productField;

        private string valueField;


        /// <remarks/>
        public AvailResponseSearch search
        {
            get
            {
                return this.searchField;
            }
            set
            {
                this.searchField = value;
            }
        }

        /// <remarks/>
        public envelopeResponseNights nights
        {
            get
            {
                return this.nightsField;
            }
            set
            {
                this.nightsField = value;
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
        public envelopeResponseEvaluate evaluate
        {
            get
            {
                return this.evaluateField;
            }
            set
            {
                this.evaluateField = value;
            }
        }

        /// <remarks/>
        public AvailResponseHotels hotels
        {
            get
            {
                return this.hotelsField;
            }
            set
            {
                this.hotelsField = value;
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
    public partial class AvailResponseSearch
    {

        private string numberField;

        private decimal timeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal time
        {
            get
            {
                return this.timeField;
            }
            set
            {
                this.timeField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseNights
    {

        private byte numberField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte number
        {
            get
            {
                return this.numberField;
            }
            set
            {
                this.numberField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class EnvelopeResponseCheckin
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
    public partial class EnvelopeResponseCheckout
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
    public partial class envelopeResponseEvaluate
    {

        private envelopeResponseEvaluateResult resultField;

        /// <remarks/>
        public envelopeResponseEvaluateResult result
        {
            get
            {
                return this.resultField;
            }
            set
            {
                this.resultField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeResponseEvaluateResult
    {

        private string codeField;

        private bool option_blockedField;

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
        public bool option_blocked
        {
            get
            {
                return this.option_blockedField;
            }
            set
            {
                this.option_blockedField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AvailResponseHotels
    {
        private AvailResponseHotel[] hotelField;

        private byte totalField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("hotel")]
        public AvailResponseHotel[] hotel
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AvailResponseHotel
    {

        private AvailHotelAgreement[] agreementField;

        private uint codeField;

        private string nameField;

        private byte starsField;

        private string locationField;

        private string addressField;

        private bool promoField;

        private string cityField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("agreement")]
        public AvailHotelAgreement[] agreement
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
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte stars
        {
            get
            {
                return this.starsField;
            }
            set
            {
                this.starsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string address
        {
            get
            {
                return this.addressField;
            }
            set
            {
                this.addressField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool promo
        {
            get
            {
                return this.promoField;
            }
            set
            {
                this.promoField = value;
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class AvailHotelAgreement
    {

        private HotelAgreementDeadline deadlineField;

        private HotelAgreementPolicy[] policiesField;

        private object deadline_remarksField;

        private HotelAgreementRemark[] remarksField;

        private HotelAgreementRoom[] roomField;

        private string idField;

        private bool availableField;

        private string room_basisField;

        private string meal_basisField;

        private ushort ctypeField;

        private ushort c_typeField;

        private bool is_dynamicField;

        private string currencyField;

        private System.DateTime deadline1Field;

        private decimal totalField;

        private decimal total_grossField;

        private decimal original_totalField;

        private bool specialField;

        private string room_typeField;

        /// <remarks/>
        public HotelAgreementDeadline deadline
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
        [System.Xml.Serialization.XmlArrayItemAttribute("policy", IsNullable = false)]
        public HotelAgreementPolicy[] policies
        {
            get
            {
                return this.policiesField;
            }
            set
            {
                this.policiesField = value;
            }
        }

        /// <remarks/>
        public object deadline_remarks
        {
            get
            {
                return this.deadline_remarksField;
            }
            set
            {
                this.deadline_remarksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("remark", IsNullable = false)]
        public HotelAgreementRemark[] remarks
        {
            get
            {
                return this.remarksField;
            }
            set
            {
                this.remarksField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("room")]
        public HotelAgreementRoom[] room
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool available
        {
            get
            {
                return this.availableField;
            }
            set
            {
                this.availableField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string room_basis
        {
            get
            {
                return this.room_basisField;
            }
            set
            {
                this.room_basisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string meal_basis
        {
            get
            {
                return this.meal_basisField;
            }
            set
            {
                this.meal_basisField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public ushort ctype
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
        public ushort c_type
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
        public string currency
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
        [System.Xml.Serialization.XmlAttributeAttribute("deadline", DataType = "date")]
        public System.DateTime deadline1
        {
            get
            {
                return this.deadline1Field;
            }
            set
            {
                this.deadline1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal total
        {
            get
            {
                return this.totalField;
            }
            set
            {
                this.totalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal total_gross
        {
            get
            {
                return this.total_grossField;
            }
            set
            {
                this.total_grossField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public decimal original_total
        {
            get
            {
                return this.original_totalField;
            }
            set
            {
                this.original_totalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool special
        {
            get
            {
                return this.specialField;
            }
            set
            {
                this.specialField = value;
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
    public partial class HotelAgreementDeadline
    {

        private string dateField;

        private string valueField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string date
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

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string value
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
    public partial class HotelAgreementPolicy
    {

        private string fromField;

        private decimal percentageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string from
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
        public decimal percentage
        {
            get
            {
                return this.percentageField;
            }
            set
            {
                this.percentageField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HotelAgreementRemark
    {

        private string codeField;

        private string textField;

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
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HotelAgreementRoom
    {

        private HotelAgreementRoomPrice[] priceField;

        private string typeField;

        private byte requiredField;

        private byte occupancyField;

        private bool cotField;

        private bool cotFieldSpecified;

        private string ageField;

        private bool extrabedField;

        private bool extrabedFieldSpecified;

        private byte occupancyInfantField;

        private bool occupancyInfantFieldSpecified;

        private byte occupancyChildField;

        private bool occupancyChildFieldSpecified;


        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("price")]
        public HotelAgreementRoomPrice[] price
        {
            get
            {
                return this.priceField;
            }
            set
            {
                this.priceField = value;
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
        public byte required
        {
            get
            {
                return this.requiredField;
            }
            set
            {
                this.requiredField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte occupancy
        {
            get
            {
                return this.occupancyField;
            }
            set
            {
                this.occupancyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool cot
        {
            get
            {
                return this.cotField;
            }
            set
            {
                this.cotField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool cotSpecified
        {
            get
            {
                return this.cotFieldSpecified;
            }
            set
            {
                this.cotFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string age
        {
            get
            {
                return this.ageField;
            }
            set
            {
                this.ageField = value;
            }
        }


        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool extrabed
        {
            get
            {
                return this.extrabedField;
            }
            set
            {
                this.extrabedField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool extrabedSpecified
        {
            get
            {
                return this.extrabedFieldSpecified;
            }
            set
            {
                this.extrabedFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte occupancyInfant
        {
            get
            {
                return this.occupancyInfantField;
            }
            set
            {
                this.occupancyInfantField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public byte occupancyChild
        {
            get
            {
                return this.occupancyChildField;
            }
            set
            {
                this.occupancyChildField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool occupancyInfantSpecified
        {
            get
            {
                return this.occupancyInfantFieldSpecified;
            }
            set
            {
                this.occupancyInfantFieldSpecified = value;
            }
        }

    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HotelAgreementRoomPrice
    {

        private HotelAgreementRoomPriceRoomprice roompriceField;

        private HotelAgreementRoomPriceRoomprice cotpriceField;

        private string fromField;

        private string toField;


        /// <remarks/>
        public HotelAgreementRoomPriceRoomprice roomprice
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

        /// <remarks/>
        public HotelAgreementRoomPriceRoomprice cotprice
        {
            get
            {
                return this.cotpriceField;
            }
            set
            {
                this.cotpriceField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")]
        public string from
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "string")]
        public string to
        {
            get
            {
                return this.toField;
            }
            set
            {
                this.toField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class HotelAgreementRoomPriceRoomprice
    {

        private decimal nettField;

        private decimal grossField;

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

}