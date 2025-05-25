namespace Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ
{
    public class AvailabilityRQ
    {
        public NetStormingAvailabilityRQ rq { get; set; }
    }
    // NOTE: Generated code may require at least .NET Framework 4.5 or .NET Core/Standard 2.0.
    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "envelope")]
    public partial class NetStormingAvailabilityRQ
    {

        private RequestEnvelopeHeader headerField;

        private AvailabilityEnvelopeQuery queryField;

        /// <remarks/>
        public RequestEnvelopeHeader header
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
        public AvailabilityEnvelopeQuery query
        {
            get
            {
                return this.queryField;
            }
            set
            {
                this.queryField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class RequestEnvelopeHeader
    {

        private string actorField;

        private string userField;

        private string passwordField;

        private string versionField;

        private string timestampField;

        /// <remarks/>
        public string actor
        {
            get
            {
                return this.actorField;
            }
            set
            {
                this.actorField = value;
            }
        }

        /// <remarks/>
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }

        /// <remarks/>
        public string password
        {
            get
            {
                return this.passwordField;
            }
            set
            {
                this.passwordField = value;
            }
        }

        /// <remarks/>
        public string version
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
        public string timestamp
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
    public partial class AvailabilityEnvelopeQuery
    {
        private envelopeQuerySearch searchField;

        private string nationalityField;

        private envelopeQueryCheckin checkinField;

        private envelopeQueryCheckout checkoutField;

        private envelopeQueryHotel[] hotelField;

        private envelopeQueryFilters filtersField;

        private envelopeQueryRoom[] detailsField;

        private string typeField;

        private string productField;

        private decimal? timeoutField;

        /// <remarks/>
        public decimal? timeout
        {
            get
            {
                return this.timeoutField;
            }
            set
            {
                this.timeoutField = value;
            }
        }
        /// <remarks/>
        public envelopeQuerySearch search
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
        public string nationality
        {
            get
            {
                return this.nationalityField;
            }
            set
            {
                this.nationalityField = value;
            }
        }

        /// <remarks/>
        public envelopeQueryCheckin checkin
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
        public envelopeQueryCheckout checkout
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
        [System.Xml.Serialization.XmlElementAttribute("hotel")]
        public envelopeQueryHotel[] hotel
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
        public envelopeQueryFilters filters
        {
            get
            {
                return this.filtersField;
            }
            set
            {
                this.filtersField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("room", IsNullable = false)]
        public envelopeQueryRoom[] details
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeQuerySearch
    {

        private string numberField;

        private string agreementField;

        private decimal priceField;

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
        public decimal price
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeQueryCheckin
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
    public partial class envelopeQueryCheckout
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
    public partial class envelopeQueryHotel
    {

        private uint idField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public uint id
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
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeQueryFilters
    {

        private string filterField;

        /// <remarks/>
        public string filter
        {
            get
            {
                return this.filterField;
            }
            set
            {
                this.filterField = value;
            }
        }
    }

    /// <remarks/>
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class envelopeQueryRoom
    {

        private string occupancyField;

        private byte requiredField;

        private bool extrabedField;

        private bool extrabedFieldSpecified;

        private bool cotField;

        private bool cotFieldSpecified;

        private string ageField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string occupancy
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
}