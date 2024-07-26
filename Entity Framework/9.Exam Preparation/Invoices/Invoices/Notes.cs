/*
1. Configure the connection string 
2. Look at the schema
3.Start the first class Model
4.Create Data Constraint class with constraint const variables
5.using static Invoices.Data.DataConstraints; to use the Data Constraints
6.Use it in Product class and everywhere else like this --> [MaxLength(ProductNameMaxLength)]
Other attributes like MinLength are working fine with DTOs
7. Create public const string ProductPriceMinValue = "5.00"; for decimal min constraint. Its saved like a string
8.Create CategoryType  enumeration in Enums folder with with possible values (ADR, Filters, Lights, Others, Tyres) required
9.Skip the navigation property for now!
10.Proceed with Address class
11.Leave the navigation properties for later
12.Invoice class -->leave navigational properties
13.Client class -->leave navigational properties
14.ProductClient class (mapping table) 
15. Add [ForeignKey(nameof(Product))] over ProductId property
16 Add public virtual Product Product { get; set; } = null!; property
17. Add .UseLazyLoadingProxies() in the OnConfiguring method
18. Same with Client
19.Create the PK with modelBuiler method
                    
                modelBuilder
                .Entity<ProductClient>()
                .HasKey(pk => new { pk.ClientId, pk.ProductId });

20 Finish the TODO: navigation properties starting with Product
Add public virtual ICollection<ProductClient> ProductsClients { get; set; }
Initialize the collection in new Hashset
21. Go to Client and add the same property public virtual ICollection<ProductClient> ProductsClients { get; set; }, initialize
22. Go to Address and add [ForeignKey(nameof(Client))] public int ClientId and public Client Client, both required
23. Go to Client and add public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>(); and Invoices
24. Go to Invoices and add 

        [Required]
        [ForeignKey(nameof(Client))]
        public int ClientId { get; set; }
        public Client Client { get; set; } = null!;

25. Last add the contexts in the InvoicesContext like:  public DbSet<Invoice> Invoices { get; set; }
26. Comment on ImportEntities and ExportEntities methods in StartUp and test the DB
27. Open File Explorer and select Data , DataProcessor, Invoices.proj, InvoicesProfile.cs, StartUp.cs  and compress it to zip for judge
************************************************ 
2.	Data Import (25pts)
1. Create Dto in ImportDto folder ImportClient Dto and AddressImportDto. Tag properties with XML attributes
2. using static Invoices.Data.DataConstraints;
3. Add more Attributes to the AddressImportDto. The MinLengthAttribute can be used in DTO!!! Add Required
Like this:
        [XmlElement("StreetName")]
        [Required]
        [MinLength(AddressStreetNameMinLength)]
        [MaxLength(AddressStreetNameMaxLength)]

4.CREATE FOLDER UTILITIES AND XmlHelper CLASS!
5.Go to Deserializer class ImportClients method and write there
Crete XmlHelper
Create Root String
Check if all data is valid with foreach and IsValid method
After that uncomment ImportEntities StartUp method and comment the other two methods for invoices.json and products.json

***************************************

Create ImportDto for the invoices.json file
We dont use Attributes when deserializing json, only with serialization
Set the properties like is shown in the example
IMPORTANT! Deserialize(import) the dates like string, not like DateTime!
IMPORTANT! Deserialize(import) the enumerations like int, not like string!
Proceed with Data Validations in ImportInvoiceDto as shown in Invoice class description from Problem 1. (Add Attributes)
using static Invoices.Data.DataConstraints;
Use [Range(InvoiceNumberMinValue, InvoiceNumberMaxValue)]
for public int Number { get; set; } Min and Max values
Validation the data in the Dto
Add required to IssueDate
[Required]
public string IssueDate { get; set; } = null!;

The DateTime validation cant be performed in the  Dto, but in the serialization
Validate the currency type in Dto
Add data constraints like 
public const int InvoiceCurrencyTypeMinValue = (int)CurrencyType.BGN;

[Required]
[Range(InvoiceCurrencyTypeMinValue,InvoiceCurrencyTypeMaxValue)]
public int CurrencyType { get; set; }

For the FK ClientId we write [Required]. He is validated in the serializer too.
After the DTO is done we are going in Deserializer class , method ImportInvoices and start implementing
After deserializing the objects, we must validate them with foreach.
DateTime must be validated with TryParse

bool isValidIssueDate = DateTime.TryParse(invoiceDto.IssueDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dateTimeIssudate);

out bool AND DateTime

for the second date DueDate
bool isValidDueDate = DateTime.TryParse(invoiceDto.DueDate,CultureInfo.InvariantCulture,DateTimeStyles.None,out DateTime dateTimeDueDate);
Validate that due date isn't before issue date with DateTime.Compare
Validate that clientId exists


Uncomment StartUp method ImportEntities to test 

                var invoices =
                DataProcessor.Deserializer.ImportInvoices(context,
                    File.ReadAllText(baseDir + "invoices.json"));
            PrintAndExportEntityToFile(invoices, exportDir + "Actual Result - ImportInvoices.txt");


************************************
Import products.json
Check DataConstraints while createting the Dto
Again use range Attribute. Auto cast to decimal from string

[Required]
[Range(typeof(decimal),ProductPriceMinValue,ProductPriceMaxValue)]


*/