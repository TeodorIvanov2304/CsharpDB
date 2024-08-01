using Medicines.Data.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Medicines.Data.DataConstraints;

namespace Medicines.Data.Models
{
    public class Medicine
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(MedicineNameMaxValue)]
        public string Name { get; set; } = null!;

        [Required]
        public decimal Price { get; set; }

        [Required]
        public Category Category { get; set; }

        [Required]
        public DateTime ProductionDate  { get; set; }

        [Required]
        public DateTime ExpiryDate  { get; set; }

        [Required]
        [MaxLength(MedicineProducerMaxValue)]
        public string Producer { get; set; }

        [Required]
        [ForeignKey(nameof(Pharmacy))]
        public int PharmacyId { get; set; }
        public Pharmacy Pharmacy { get; set; }
        public ICollection<PatientMedicine> PatientsMedicines { get; set; }
    }
}
