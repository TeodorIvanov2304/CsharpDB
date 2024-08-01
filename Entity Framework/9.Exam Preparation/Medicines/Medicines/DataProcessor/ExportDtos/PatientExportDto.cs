using Medicines.Data.Models.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml.Serialization;

namespace Medicines.DataProcessor.ExportDtos
{
    [XmlType("Patient")]
    public class PatientExportDto
    {
        [XmlAttribute("Gender")]
        public string Gender { get; set; } = null!;

        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("AgeGroup")]
        public AgeGroup AgeGroup { get; set; }

        [XmlArray("Medicines")]
        public PatientMedicineExportDto[] Medicines { get; set; }
    }
}
