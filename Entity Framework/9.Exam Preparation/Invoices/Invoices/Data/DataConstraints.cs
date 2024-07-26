using Invoices.Data.Models.Enums;

namespace Invoices.Data
{
    public static class DataConstraints
    {   
        //Product
        public const byte ProductNameMinLength = 9;
        public const byte ProductNameMaxLength = 30;
        public const string ProductPriceMinValue = "5.00";
        public const string ProductPriceMaxValue = "1000.00";
        public const int ProductCategoryTypeMinValue = (int)CategoryType.ADR;
        public const int ProductCategoryTypeMaxValue = (int)CategoryType.Tyres;

        //Address
        public const byte AddressStreetNameMinLength = 10;
        public const byte AddressStreetNameMaxLength = 20;
        public const byte CityNameMinLength = 5;
        public const byte CityNameMaxLength = 15;
        public const byte CountryNameMinLength = 5;
        public const byte CountryNameMaxLength = 15;

        //Invoice
        public const int InvoiceNumberMinValue = 1_000_000_000;
        public const int InvoiceNumberMaxValue = 1_500_000_000;
        public const int InvoiceCurrencyTypeMinValue = (int)CurrencyType.BGN; // 0
        public const int InvoiceCurrencyTypeMaxValue = (int)CurrencyType.USD; // 2

        //Client
        public const byte ClientNameMinLength = 10;
        public const byte ClientNameMaxLength = 25;
        public const byte ClientNumberVatMinLength = 10;
        public const byte ClientNumberVatMaxLength = 15;
    }
}
