namespace Infrastructure.Connectivity.Connector.Models.Message.Common
{
    public class BaseFromTo
    {
        private string dateFrom;
        private string dateTo;

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string from
        {
            get
            {
                return this.dateFrom;
            }
            set
            {
                this.dateFrom = value;
            }
        }
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string to
        {
            get
            {
                return this.dateTo;
            }
            set
            {
                this.dateTo = value;
            }
        }
    }
}
