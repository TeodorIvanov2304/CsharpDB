using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]   
    
    public class ExportGuideWithSpanish
    {
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [XmlArray("TourPackages")]
        public ExportGuideTourPackages[] TourPackages { get; set; } = new ExportGuideTourPackages[0];
    }
}
