using Medicines.Data.Models.Enums;

namespace Medicines.Data
{
    public static class DataConstraints
    {
        //Pharmacy
        public const int PharmacyNameMinLength = 2;
        public const int PharmacyNameMaxLength = 50;
        public const string PharmacyIsNonStopFalseValue = "false";
        public const string PharmacyIsNonStopTrueValue = "true";

        //Medicine
        public const string MedicinePriceMinValue = "0.01";
        public const string MedicinePriceMaxValue = "1000.00";
        public const int MedicineNameMinValue = 3;
        public const int MedicineNameMaxValue = 150;
        public const int MedicineProducerMinValue = 3;
        public const int MedicineProducerMaxValue = 100;
        public const int MedicineCategoryMinValue = (int)Category.Analgesic;
        public const int MedicineCategoryMaxValue = (int)Category.Vaccine;


        //Patient
        public const int PatientFullNameMinValue = 5;
        public const int PatientFullNameMaxValue = 100;
    }
}
