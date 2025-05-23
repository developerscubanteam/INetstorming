using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

namespace System
{
    public enum NameHandling
    {
        None = 0,
        Objects = 1,
        Arrays = 2,
        All = 3,
        Auto = 4
    }
    public static class SerializeExtension
    {

        public static string CleanSoap(this string s)
        {
            return s.Replace("<S:Envelope xmlns:S=\"http://schemas.xmlsoap.org/soap/envelope/\">", "")
                .Replace("<S:Body>", "")
                .Replace("</S:Body>", "")
                .Replace("</S:Envelope>", "");
        }

        public static string GenerateSoapMessage<T>(this T requestObject)
        {
            string serializedRequest = requestObject.SerializeObjectToXmlString();
            string allNamespaces = string.Empty;
            //add correct namespaces
            string soapMessage = $@"
<soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:blac=""http://topdog.uk.net/BlackBox"">
<soapenv:Header/>
<soapenv:Body>{serializedRequest}
    </soapenv:Body>
</soapenv:Envelope>";

            return soapMessage;
        }

        public static T DeserializateObjectToXmlString<T>(this string data)
        {
            Type objectType = typeof(T);

            string objectNamespace = GetXmlNamespace(objectType);
            string rootElementName = GetXmlRootElementName(objectType);
            var xmlRoot = new XmlRootAttribute(rootElementName)
            {
                Namespace = objectNamespace
            };
            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);
            T responseData = default(T);
            using (var reader = new StringReader(data))
            {
                try
                {
                    // Intenta la deserialización
                    responseData = (T)serializer.Deserialize(reader);

                    return responseData;

                }
                catch (InvalidOperationException ioEx)
                {
                    // Captura el error específico si la deserialización falla
                    return default(T);
                }
                catch (Exception ex)
                {
                    return default(T);
                }
            }


        }

        public static string SerializeObjectToXmlString<T>(this T toSerialize, bool useIndent = false, bool omitStandardNamespaces = true)
        {
            if (toSerialize == null)
            {
                return string.Empty;
            }

            Type objectType = typeof(T);

            string objectNamespace = GetXmlNamespace(objectType);
            string rootElementName = GetXmlRootElementName(objectType);

            var xmlRoot = new XmlRootAttribute(rootElementName)
            {
                Namespace = objectNamespace

            };
            XmlSerializer ser = new XmlSerializer(typeof(T), xmlRoot);
            var namespaces = new XmlSerializerNamespaces();
            if (!string.IsNullOrEmpty(objectNamespace))
            {
                namespaces.Add("", objectNamespace);
            }
            else if (omitStandardNamespaces)
            {
                namespaces.Add("", "");
            }
            var utf8EncodingWithoutBom = new UTF8Encoding(false);
            var settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = useIndent,
                Encoding = utf8EncodingWithoutBom,
            };
            using (MemoryStream ms = new MemoryStream())
            {
                using (XmlWriter writer = XmlWriter.Create(ms, settings))
                {

                    ser.Serialize(writer, toSerialize, namespaces);
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static string GetXmlNamespace(Type type)
        {
            var xmlTypeAttr = type.GetCustomAttribute<XmlTypeAttribute>();
            if (xmlTypeAttr != null && !string.IsNullOrEmpty(xmlTypeAttr.Namespace))
            {
                return xmlTypeAttr.Namespace;
            }
            var xmlRootAttr = type.GetCustomAttribute<XmlRootAttribute>();
            if (xmlRootAttr != null && !string.IsNullOrEmpty(xmlRootAttr.Namespace))
            {
                return xmlRootAttr.Namespace;
            }
            return null; // No se encontró namespace en los atributos comunes
        }
        private static string GetXmlRootElementName(Type type)
        {
            var xmlRootAttr = type.GetCustomAttribute<XmlRootAttribute>();
            if (xmlRootAttr != null && !string.IsNullOrEmpty(xmlRootAttr.ElementName))
                return xmlRootAttr.ElementName;

            // Por defecto, usa el nombre de la clase si no hay XmlRootAttribute especificándolo
            return type.Name;
        }

        public static string SerializeObjectXml<T>(this T toSerialize)
        {  
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (var ms = new MemoryStream())
            {
                using (XmlTextWriter xmlTextWriter = new XmlTextWriter(ms, Encoding.UTF8))
                {
                    xmlTextWriter.Formatting = Xml.Formatting.None;
                    xmlSerializer.Serialize(xmlTextWriter, toSerialize);
                    xmlTextWriter.Flush();
                    xmlTextWriter.BaseStream.Position = 0;
                    StreamReader sr = new StreamReader(xmlTextWriter.BaseStream);
                    var restulXml = sr.ReadToEnd();
                    sr.Close();
                    return restulXml;
                }
            }
        }

        public static T DeserializeXml<T>(this string thisString)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(thisString);
                writer.Flush();
                stream.Position = 0;
                var result = (T)xmlSerializer.Deserialize(stream);

                return result;
            }
        }

        public static JsonSerializerOptions Configure()
        {
            JsonSerializerOptions options = new JsonSerializerOptions();
            options.Converters.Add(new AllAsStringConverter());
            return options;
        }

    }

    public class AllAsStringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            //Debug.Assert(typeToConvert == typeof(long) || typeToConvert == typeof(float)
            //    || typeToConvert == typeof(decimal) || typeToConvert == typeof(DateTime));
            using (var jsonDoc = JsonDocument.ParseValue(ref reader))
            {
                var result = jsonDoc.RootElement.GetRawText().Replace("\"", "");
                if (result.StartsWith("\""))
                {
                    result = result.Substring(1);
                }
                if (result.EndsWith("\""))
                {
                    result = result.Substring(0, result.Length - 1);
                }
                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
