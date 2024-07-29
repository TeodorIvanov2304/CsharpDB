using Boardgames.Data.Models.Enums;

namespace Boardgames.Data
{
    public static class DataConstraints
    {
        //Creator
        public const byte CreatorFirstNameMinLength = 2;
        public const byte CreatorFirstNameMaxLength = 7;
        public const byte CreatorLastNameMinLength = 2;
        public const byte CreatorLastNameMaxLength = 7;

        //Boardgame
        public const byte BoardGameNameMinLength = 10;
        public const byte BoardGameNameMaxLength = 20;
        public const double BoardGameRatingMinValue = 1.00;
        public const double BoardGameRatingMaxValue = 10.00;
        public const int BoardGameYearPublishedMinValue = 2018;
        public const int BoardGameYearPublishedMaxValue = 2023;
        public const int BoardGameCategoryTypeMinValue = (int)CategoryType.Abstract;
        public const int BoardGameCategoryTypeMaxValue = (int)CategoryType.Strategy;

        //Seller
        public const byte SellerNameMinLength = 5;
        public const byte SellerNameMaxLength = 20;
        public const byte SellerAddressMinLength = 2;
        public const byte SellerAddressMaxLength = 30;

    }
}
