namespace Medicines.DataProcessor
{
    using Medicines.Data;
    using Medicines.Data.Models;
    using static Medicines.Data.DataConstraints;
    using Medicines.DataProcessor.ImportDtos;
    using Medicines.Utilities;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Newtonsoft.Json;
    using Medicines.Data.Models.Enums;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid Data!";
        private const string SuccessfullyImportedPharmacy = "Successfully imported pharmacy - {0} with {1} medicines.";
        private const string SuccessfullyImportedPatient = "Successfully imported patient - {0} with {1} medicines.";

        public static string ImportPatients(MedicinesContext context, string jsonString)
        {
            StringBuilder sb = new();
            PatientImportDto[] patients = JsonConvert.DeserializeObject<PatientImportDto[]>(jsonString)!;
            ICollection<Patient> patientsToImport = new List<Patient>();

            foreach (var patientDto in patients)
            {
                ICollection<PatientMedicine> medicinesToImport = new List<PatientMedicine>();
                if (!IsValid(patientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Patient newPatient = new Patient()
                {
                    FullName = patientDto.FullName,
                    AgeGroup = (AgeGroup)patientDto.AgeGroup,
                    Gender = (Gender)patientDto.Gender,
                    PatientsMedicines = new List<PatientMedicine>()
                };

                foreach (var medicine in patientDto.Medicines)
                {
                    if (!IsValid(medicine))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    if (medicinesToImport.Any(m => m.MedicineId == medicine && m.PatientId == newPatient.Id))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    PatientMedicine newPatientMedicine = new PatientMedicine()
                    {
                        MedicineId = medicine,
                        PatientId = newPatient.Id,
                    };
                    newPatient.PatientsMedicines.Add(newPatientMedicine);
                    medicinesToImport.Add(newPatientMedicine); 
                }

                patientsToImport.Add(newPatient);
                sb.AppendLine(String.Format(SuccessfullyImportedPatient, newPatient.FullName, newPatient.PatientsMedicines.Count));
            }

            context.Patients.AddRange(patientsToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportPharmacies(MedicinesContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Pharmacies";

            PharmacyImportDto[] pharmacyDtos = xmlHelper.Deserialize<PharmacyImportDto[]>(xmlString, xmlRoot);

            List<Pharmacy> pharmaciesToImport = new List<Pharmacy>();

            foreach (var pharmacyDto in pharmacyDtos)
            {
                if (pharmacyDto.IsNonStop.ToString() != PharmacyIsNonStopFalseValue && pharmacyDto.IsNonStop.ToString() != PharmacyIsNonStopTrueValue)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!IsValid(pharmacyDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                List<Medicine> medicinesToImport = new List<Medicine>();

                foreach (var medicineDto in pharmacyDto.Medicines)
                {
                    if (!IsValid(medicineDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    DateTime productionDate = DateTime.ParseExact(medicineDto.ProductionDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime expiryDate = DateTime.ParseExact(medicineDto.ExpiryDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    if (DateTime.Compare(productionDate, expiryDate) >= 0)
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

             
                    if (medicinesToImport.Any(m => m.Name == medicineDto.Name && m.Producer == medicineDto.Producer))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Medicine newMedicine = new Medicine()
                    {   
                        Category = (Category)medicineDto.Category,
                        Name = medicineDto.Name,
                        Price = medicineDto.Price,
                        ProductionDate = productionDate,
                        ExpiryDate = expiryDate,
                        Producer = medicineDto.Producer,
                    };

                    medicinesToImport.Add(newMedicine);
                }

                Pharmacy newPharmacy = new Pharmacy()
                {   
                    IsNonStop = bool.Parse(pharmacyDto.IsNonStop),
                    Name = pharmacyDto.Name,
                    PhoneNumber = pharmacyDto.PhoneNumber,
                    Medicines = medicinesToImport
                };

                pharmaciesToImport.Add(newPharmacy);
                sb.AppendLine(String.Format(SuccessfullyImportedPharmacy, newPharmacy.Name, newPharmacy.Medicines.Count));
            }

            context.Pharmacies.AddRange(pharmaciesToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
