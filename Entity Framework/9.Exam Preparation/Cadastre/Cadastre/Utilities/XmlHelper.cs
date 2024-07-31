using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace Cadastre.Utilities
{
    public class XmlHelper
    {
        //Deserializing method
        public T Deserialize<T>(string inputXml, string rootName)
            where T : class
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute(rootName);
            using StringReader reader = new StringReader(inputXml);
            XmlSerializer serializer = new XmlSerializer(typeof(T), xmlRoot);
            object? deserializedObjects = serializer.Deserialize(reader);
            if (deserializedObjects == null || deserializedObjects is not T deserializedObjectTypes)
            {
                throw new InvalidOperationException();
            }

            return deserializedObjectTypes;
        }

        //Second Deserializier
        public T Deserialize2<T>(string inputXml, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);
            using StringReader reader = new StringReader(inputXml);
            T? obj = (T)serializer.Deserialize(reader);

            return obj;
        }


        //Serialize
        public string Serialize<T>(T obj, string rootName)
        {
            XmlRootAttribute root = new XmlRootAttribute(rootName);
            XmlSerializer serializer = new XmlSerializer(typeof(T), root);

            var settings = new XmlWriterSettings()
            {
                Encoding = new UTF8Encoding(false),
                Indent = true,
                Async = true,
            };
            //Remove unnecessary namespaces
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);
            StringBuilder sb = new StringBuilder();
            using StringWriter writer = new StringWriter(sb);


            serializer.Serialize(writer, obj, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Serializing method!!! 2
        public static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true,
                Async = true,
            };

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            try
            {
                xmlSerializer.Serialize(stringWriter, dto, xmlSerializerNamespaces);
            }
            catch (Exception)
            {

                throw new ArgumentException("Error with serialization");
            }


            return stringBuilder.ToString().TrimEnd();
        }
    }
}
