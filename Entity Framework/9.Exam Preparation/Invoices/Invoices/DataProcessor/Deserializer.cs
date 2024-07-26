namespace Invoices.DataProcessor
{
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Text;
    using Invoices.Data;
    using Invoices.Data.Models;
    using Invoices.Data.Models.Enums;
    using Invoices.DataProcessor.ImportDto;
    using Invoices.Utilities;
    using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedClients
            = "Successfully imported client {0}.";

        private const string SuccessfullyImportedInvoices
            = "Successfully imported invoice with number {0}.";

        private const string SuccessfullyImportedProducts
            = "Successfully imported product - {0} with {1} clients.";


        public static string ImportClients(InvoicesContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlHelper helper = new XmlHelper();
            const string xmlRoot = "Clients";

            ICollection<Client> clientsToImport = new List<Client>();

            ImportClientDto[] deserializedDtos =
                helper.Deserialize2<ImportClientDto[]>(xmlString, xmlRoot);
            foreach (ImportClientDto clientDto in deserializedDtos)
            {
                if (!IsValid(clientDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                ICollection<Address> addressesToImport = new List<Address>();
                foreach (AddressImportDto clientDtoAddress in clientDto.Addresses)
                {
                    if (!IsValid(clientDtoAddress))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }
                    Address address = new Address()
                    {
                        City = clientDtoAddress.City,
                        StreetName = clientDtoAddress.StreetName,
                        StreetNumber = clientDtoAddress.StreetNumber,
                        PostCode = clientDtoAddress.PostCode,
                        Country = clientDtoAddress.Country
                    };
                    addressesToImport.Add(address);
                }
                Client newClient = new Client()
                {
                    Name = clientDto.Name,
                    NumberVat = clientDto.NumberVat,
                    Addresses = addressesToImport
                };
                clientsToImport.Add(newClient);
                sb.AppendLine(string.Format(SuccessfullyImportedClients, clientDto.Name));
            }
            context.Clients.AddRange(clientsToImport);
            context.SaveChanges();
            return sb.ToString();
        }


        public static string ImportInvoices(InvoicesContext context, string jsonString)
        {
            StringBuilder sb = new();
            ImportInvoiceDto[] deserializedInvoices = JsonConvert.DeserializeObject<ImportInvoiceDto[]>(jsonString)!;
            ICollection<Invoice> invoices = new List<Invoice>();
            
            foreach (var invoiceDto in deserializedInvoices)
            {
                if (!IsValid(invoiceDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                bool isValidIssueDate = DateTime.TryParse(invoiceDto.IssueDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime issueDate);
                
                bool isValidDueDate = DateTime.TryParse(invoiceDto.DueDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dueDate);

                if (isValidIssueDate == false 
                    || isValidDueDate == false
                    || DateTime.Compare(dueDate,issueDate) < 0)
                {   

                    //DateTime.Compare(t1, t2)
                    //-> -1  - t1 is before t2
                    //-> 0   - t1 is t2
                    //-> 1   - t1 is after t2
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                if (!context.Clients.Any(cl=> cl.Id == invoiceDto.ClientId))
                {
                    //Non-existent client
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Invoice newInvoice = new Invoice()
                {
                    Number = invoiceDto.Number,
                    IssueDate = issueDate,
                    DueDate = dueDate,
                    Amount = invoiceDto.Amount,
                    CurrencyType = (CurrencyType)invoiceDto.CurrencyType,
                    ClientId = invoiceDto.ClientId,
                };

                invoices.Add(newInvoice);
                sb.AppendLine(String.Format(SuccessfullyImportedInvoices, invoiceDto.Number));
            }
            context.Invoices.AddRange(invoices);
            context.SaveChanges();
            return sb.ToString();
        }

        public static string ImportProducts(InvoicesContext context, string jsonString)
        {


            StringBuilder sb = new();
            ICollection<Product> productsToImport = new List<Product>();

            ImportProductDto[] deserializedProducts = JsonConvert.DeserializeObject<ImportProductDto[]>(jsonString);


            foreach (var productDto in deserializedProducts)
            {
                if (!IsValid(productDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Product newProduct = new Product()
                {
                    Name = productDto.Name,
                    Price = productDto.Price,
                    CategoryType = (CategoryType)productDto.CategoryType
                };

                ICollection<ProductClient> productClientsToImport = new List<ProductClient>();
                foreach (int clientId in productDto.Clients.Distinct())
                {
                    if (!context.Clients.Any(cl => cl.Id == clientId))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    ProductClient productClient = new ProductClient()
                    {   
                        Product = newProduct,
                        ClientId = clientId
                    };

                    productClientsToImport.Add(productClient);
                }

                newProduct.ProductsClients = productClientsToImport;

                productsToImport.Add(newProduct);
                sb.AppendLine(String.Format(SuccessfullyImportedProducts, productDto.Name, productClientsToImport.Count));
            }

            context.Products.AddRange(productsToImport);
            context.SaveChanges();

            return sb.ToString();
        }

        public static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    } 
}
