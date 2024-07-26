using System.ComponentModel.DataAnnotations;
using static Invoices.Data.DataConstraints;

namespace Invoices.DataProcessor.ImportDto
{
    public class ImportInvoiceDto
    {
        [Required]
        [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
        public int Number { get; set; }
        [Required]
        public string IssueDate { get; set; } = null!; //DateTime --> Deserialize as a string
        [Required]
        public string DueDate { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; }

        [Required]
        [Range(InvoiceCurrencyTypeMinValue,InvoiceCurrencyTypeMaxValue)]
        public int CurrencyType { get; set; } //Enumeration -->Deserialize as a int
        [Required]
        public int ClientId { get; set; }
    }
}
