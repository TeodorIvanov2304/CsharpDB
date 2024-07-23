﻿using System.Xml.Serialization;

namespace CarDealer.DTOs.Export
{
    [XmlType("customer")]
    public class CustomerWithCarExportDto
    {
        [XmlAttribute("full-name")]
        public string FullName { get; set; }

        [XmlAttribute("bought-cars")]
        public int CarsBought { get; set; }

        [XmlAttribute("spent-money")]
        public decimal SpentMoney { get; set; }

    }
}