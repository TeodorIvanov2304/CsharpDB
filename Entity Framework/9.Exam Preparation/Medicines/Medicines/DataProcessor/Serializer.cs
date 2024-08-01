namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models.Enums;
    using Medicines.DataProcessor.ExportDtos;
    using Medicines.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;
    using System.Text.Json.Nodes;

    public class Serializer
    {
        public static string ExportPatientsWithTheirMedicines(MedicinesContext context, string date)
        {

            DateTime givenDate = DateTime.ParseExact(date, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            var patientsWithMedicines = context.Patients
         .Where(p => p.PatientsMedicines.Any(pm => pm.Medicine.ProductionDate > givenDate))
         .Select(p => new PatientExportDto()
         {
             Name = p.FullName,
             AgeGroup = p.AgeGroup,
             Gender = p.Gender.ToString().ToLower(),
             Medicines = p.PatientsMedicines
                         .Where(pm => pm.Medicine.ProductionDate > givenDate)
                         .OrderByDescending(pm => pm.Medicine.ExpiryDate)
                         .ThenBy(pm => pm.Medicine.Price)
                         .Select(pm => new PatientMedicineExportDto()
                         {
                             Name = pm.Medicine.Name,
                             Category = pm.Medicine.Category.ToString().ToLower(),
                             Price = pm.Medicine.Price.ToString("F2", CultureInfo.InvariantCulture),
                             Producer = pm.Medicine.Producer,
                             BestBefore = pm.Medicine.ExpiryDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)
                         })
                         .ToArray()
         })
         .OrderByDescending(p => p.Medicines.Length)
         .ThenBy(p => p.Name)
         .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            const string root = "Patients";


            return xmlHelper.Serialize(patientsWithMedicines, root);
        }

        public static string ExportMedicinesFromDesiredCategoryInNonStopPharmacies(MedicinesContext context, int medicineCategory)
        {
            var medicinesInNonStopPharmacies = context.Medicines
                .Where(m => m.Pharmacy.IsNonStop)
                .Where(m => (int)m.Category == medicineCategory)
                .OrderBy(m => m.Price)
                .ThenBy(m => m.Name)
                .Select(dto => new ExportMedicineDto()
                {
                    Name = dto.Name,
                    Price = $"{dto.Price:F2}",
                    Pharmacy = new ExportPharmacyDto()
                    {
                        Name = dto.Pharmacy.Name,
                        PhoneNumber = dto.Pharmacy.PhoneNumber
                    }

                })
                .ToList();

            return JsonConvert.SerializeObject(medicinesInNonStopPharmacies, Formatting.Indented);
        }
    }
}
