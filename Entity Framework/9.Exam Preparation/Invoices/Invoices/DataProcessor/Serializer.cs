namespace Invoices.DataProcessor
{
    using Invoices.Data;
    using Invoices.DataProcessor.ExportDto;
    using Invoices.Utilities;
    using Newtonsoft.Json;
    using System.Globalization;

    public class Serializer
    {
        public static string ExportClientsWithTheirInvoices(InvoicesContext context, DateTime date)
        {   
            XmlHelper xmlHelper= new XmlHelper();
            const string xmlRoot = "Clients";

            var clientsToExport = context.Clients
                .Where(c => c.Invoices.Any(i => DateTime.Compare(i.IssueDate,date) > 0))
                .Select(dto => new ExportClientWithInvoicesDto()
                {
                    ClientName = dto.Name,
                    VatNumber = dto.NumberVat,
                    Invoices = dto.Invoices
                    .OrderBy(i => i.IssueDate)
                    .ThenByDescending(i => i.DueDate)
                    .Select(inv => new ExportInvoiceDto() 
                    {
                        InvoiceNumber = inv.Number,
                        InvoiceAmount = inv.Amount,
                        Currency = inv.CurrencyType.ToString(),
                        DueDate = inv.DueDate.ToString("d",CultureInfo.InvariantCulture)
                    })
                    .ToArray(),
                    InvoicesCount = dto.Invoices.Count()
                })
                .OrderByDescending(dto=>dto.InvoicesCount)
                .ThenBy(dto=>dto.ClientName)
                .ToArray();

            return xmlHelper.Serialize(clientsToExport,xmlRoot);
        }

        public static string ExportProductsWithMostClients(InvoicesContext context, int nameLength)
        {

            ExportProductDto[] productsToExport = context.Products
                .Where(p => p.ProductsClients.Any())
                .Where(p => p.ProductsClients.Any(n => n.Client.Name.Length >= nameLength))
                .Select(p => new ExportProductDto 
                {
                    Name = p.Name,
                    Price = p.Price,
                    Category = p.CategoryType.ToString(),
                    Clients = p.ProductsClients.Where(pc=>pc.Client.Name.Length >= nameLength)
                                               .Select(pc => new ExportProductClientsDto ()
                                               {
                                                   Name = pc.Client.Name,
                                                   NumberVat = pc.Client.NumberVat,
                                               })
                                               .OrderBy(c=>c.Name)
                                               .ToArray()
                })
                .OrderByDescending(p => p.Clients.Length)
                .ThenBy(p => p.Name)
                .Take(5)
                .ToArray();

            
            return JsonConvert.SerializeObject(productsToExport, Formatting.Indented);
        }
    }
}