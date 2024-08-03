using TravelAgency.Data.Models.Enums;

namespace TravelAgency.Data
{
    public static class DataConstraints
    {
        //Customer
        public const int CustomerFullNameMinValue = 4;
        public const int CustomerFullNameMaxValue = 60;
        public const int CustomerEmailMinValue = 6;
        public const int CustomerEmailMaxValue = 50;

        //Guide
        public const int GuideFullNameMinValue = 4;
        public const int GuideFullNameMaxValue = 60;
        public const int GuideLanguageMinValue = (int)Language.English;
        public const int GuideLanguageMaxValue = (int)Language.Russian;

        //TourPackage
        public const int TourPackageNameMinValue = 2;
        public const int TourPackageNameMaxValue = 40;
        public const int TourPackageDescriptionMaxValue = 200;
    }
}
