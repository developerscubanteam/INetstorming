using System.Xml;

namespace Infrastructure.Connectivity.Connector.Models.Message.Common
{
    internal interface IDeserializer
    {
        void DeserializeFromXML(XmlReader reader);
    }
}
