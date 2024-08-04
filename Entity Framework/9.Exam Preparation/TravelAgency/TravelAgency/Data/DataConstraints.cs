using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data
{
    public static class DataConstraints
    {
        //Customer
        public const byte CustomerFullNameMinValue = 4;
        public const byte CustomerFullNameMaxValue = 60;
        public const byte CustomerEmailMinValue = 6;
        public const byte CustomerEmailMaxValue = 50;

        //Guide
        public const byte GuideFullNameMinValue = 4;
        public const byte GuideFullNameMaxValue = 60;
        public const int GuideLanguageMinValue = (int)Language.English;
        public const int GuideLanguageMaxValue = (int)Language.Russian;

        //TourPackage
        public const byte TourPackageNameMinValue = 2;
        public const byte TourPackageNameMaxValue = 40;
        public const byte TourPackageDescriptionMaxValue = 200;

    }
}
