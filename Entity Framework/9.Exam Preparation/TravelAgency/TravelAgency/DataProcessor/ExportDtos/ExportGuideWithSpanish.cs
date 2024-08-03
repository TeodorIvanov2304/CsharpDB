using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]   
    
    public class ExportGuideWithSpanish
    {
        [XmlElement("FullName")]
        public string FullName { get; set; }

        [XmlArray("TourPackages")]
        public ExportGuideTourPackages[] TourPackages { get; set; }
    }
}
