
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Medicines.Utilities
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

            XmlWriterSettings settings = new();
            settings.OmitXmlDeclaration = true;
            settings.IndentChars = "\t";
            settings.Indent = true;

            StringWriter stringWriter = new();


            using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
            {
                xmlSerializer.Serialize(writer, objectToSerialize, ns);

                return writer.ToString();
            }



        }
    }
}
