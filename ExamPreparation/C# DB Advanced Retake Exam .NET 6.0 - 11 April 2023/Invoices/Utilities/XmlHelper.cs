using Invoices.DataProcessor.ExportDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Invoices.Utilities
{
    public class XmlHelper
    {
        public T Decerialize<T>(string inputXml, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T), xmlRoot);

            using StringReader reader = new StringReader(inputXml);
            T decerializedDtos = (T)xmlSerializer.Deserialize(reader);

            return decerializedDtos;
        }

        public string Serialize<T>(T objectToSerialize, string rootName)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            XmlSerializer xmlSerializer = new XmlSerializer(objectToSerialize.GetType(), xmlRoot);
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, objectToSerialize, ns);

                return writer.ToString();
            }

        }
    }
}
