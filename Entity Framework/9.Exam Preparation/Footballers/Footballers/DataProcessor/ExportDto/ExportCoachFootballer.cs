﻿using System.Xml.Serialization;

namespace Footballers.DataProcessor.ExportDto
{
    [XmlType("Footballer")]
    public class ExportCoachFootballer
    {
        [XmlElement("Name")]         
        public string Name { get; set; }

        [XmlElement("Position")]
        public string Position { get; set; }
    }
}
