using Footballers.Data.Models.Enums;
using static Footballers.Data.DataConstraints;

namespace Footballers.Data
{
    public static class DataConstraints
    {   
        //Footballer
        public const byte FootballerNameMinValue = 2;
        public const byte FootballerNameMaxValue = 40;
        public const int FootballerPositionTypeMinValue = (int)PositionType.Goalkeeper;
        public const int FootballerPositionTypeMaxValue = (int)PositionType.Forward;
        public const int FootballerBestSkillTypeMinValue = (int)BestSkillType.Defence;
        public const int FootballerBestSkillTypeMaxValue = (int)BestSkillType.Speed;

        //Team
        public const byte TeamNameMinValue = 3;
        public const byte TeamNameMaxValue = 40;
        public const byte TeamNationalityMinValue = 2;
        public const byte TeamNationalityMaxValue = 40;

        //Coach
        public const byte CoachNameMinValue = 2;
        public const byte CoachNameMaxValue = 40;
    }
}
