using System.ComponentModel.DataAnnotations;
using static Medicines.Data.DataConstraints;

namespace Medicines.DataProcessor.ExportDtos
{
    public class ExportMedicineDto
    {
        public string Name { get; set; } = null!;      
        public string Price { get; set; }
        public ExportPharmacyDto Pharmacy { get; set; }
    }
}
