using System.Text.Json.Serialization;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using XMLtoJasonDemo.Models;

string xml = @"<?xml version=""1.0"" standalone=""no""?>
<root>
    <person id=""1"">
        <name>Alan</name>
        <url>www.google.com</url>
    </person>
    <person id=""2"">
        <name>Louis</name>
        <url>www.yahoo.com</url>
    </person>
</root>";

//Create XML document
XmlDocument xmlDocument = new XmlDocument();

//Load XML string
xmlDocument.LoadXml(xml);

//Serialize to JSON and use Indented formatting
string data = JsonConvert.SerializeXmlNode(xmlDocument,Newtonsoft.Json.Formatting.Indented);

Console.WriteLine(data);
Console.WriteLine("************************************************");

Person person = new Person()
{
    FullName = "Miko Mikov",
    Age = 43,
    Height = 177,
    Weight = 80,
    Address = new Address() 
    {
        City = "Varna",
        Street = "Cherno more"
    }
};

string dataForJObject = JsonConvert.SerializeObject(person,Newtonsoft.Json.Formatting.Indented);
JObject jObject  = JObject.Parse(dataForJObject);

Console.WriteLine(data);