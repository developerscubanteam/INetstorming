using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRQ;
using Infrastructure.Connectivity.Connector.Models.Message.AvailabilityRS;

namespace Infrastructure.Connectivity.Connector.Models.Message.Common
{
    [System.SerializableAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false, ElementName = "envelope")]
    public partial class NetstormingGetDeadLineRS
    {

        private RequestEnvelopeHeader headerField;

        private GetDeadlineEnvelopeResponse responseField;

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
        public GetDeadlineEnvelopeResponse response
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
    public partial class GetDeadlineEnvelopeResponse
    {

        private DeadlineAvailability availabilityField;

        private GetDeadLineAgreement agreementField;

        private HotelAgreementDeadline deadlineField;

        private HotelAgreementPolicy[] policiesField;

        private HotelAgreementRemark[] remarksField;

        private string typeField;

        private string productField;

        /// <remarks/>
        public DeadlineAvailability availability
        {
            get
            {
                return this.availabilityField;
            }
            set
            {
                this.availabilityField = value;
            }
        }

        /// <remarks/>
        public GetDeadLineAgreement agreement
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
}
