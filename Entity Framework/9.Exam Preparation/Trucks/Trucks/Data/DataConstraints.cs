using Trucks.Data.Models.Enums;

namespace Trucks.Data
{
    public static class DataConstraints
    {
        //Truck
        public const byte TruckRegistrationNumberLength = 8;
        public const byte TruckVinNumberLength = 17;
        public const int TruckTankCapacityMinLength = 950;
        public const int TruckTankCapacityMaxLength = 1420;
        public const int TruckCargoCapacityMinLength = 5_000;
        public const int TruckCargoCapacityMaxLength = 29_000;
        public const int TruckCategoryTypeMinValue = (int)CategoryType.Flatbed;
        public const int TruckCategoryTypeMaxValue = (int)CategoryType.Semi;
        public const int TruckMakeTypeMinValue = (int)MakeType.Daf;
        public const int TruckMakeTypeMaxValue = (int)MakeType.Volvo;

        //Client
        public const byte ClientNameMinLength = 3;
        public const byte ClientNameMaxLength = 40;
        public const byte ClientNationalityMinLength = 2;
        public const byte ClientNationalityMaxLength = 40;

        //Despatcher
        public const byte DespatcherNameMinLength = 2;
        public const byte DespatcherNameMaxLength = 40;

    }
}
