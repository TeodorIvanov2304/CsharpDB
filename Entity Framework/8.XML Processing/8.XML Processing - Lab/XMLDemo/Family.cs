using System.Xml.Serialization;

namespace XMLDemo
{
    public class Family
    {
        [XmlAttribute("name")]
        public string FamilyName { get; set; }
        [XmlElement("members")]
        public Person[] Members { get; set; } 
    }
}
