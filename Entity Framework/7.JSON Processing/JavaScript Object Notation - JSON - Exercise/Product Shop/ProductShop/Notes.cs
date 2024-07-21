/*
Start with Add-Migration InitialMigration
Update-Database
Fix error regarding FOREIGN KEY constraint 'FK_Products_Users_SellerId' that  may cause cycles or multiple cascade paths
Change in migration file onDelete: ReferentialAction.Cascade); and change it to onDelete: ReferentialAction.NoAction);
Update-Database

Import Users
Create Db set
create string file and read the text from users.json using File.ReadAllText
Deserialize the string in type List<User>;
Add the list of users with context.Users.AddRange(users);
Fix error in Product class :
Change public User Buyer { get; set; } = null!; to public User? Buyer { get; set; }
Add new migration and Update-Database


For Problem 5
We create ExportProductDto class int DTOs/Export

"name" is the desired JSON variable name
Then we make  the query:
var productsInRange = context.Products
                .Where(p=>p.Price >=500 && p.Price <=1000)
                .Select(p=> new ExportProductDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Seller = $"{p.Seller.FirstName} {p.Seller.LastName}"
                })
                .OrderBy(p=>p.Price)
                .ToArray();

We use the new ExportProductDto() instead anonymous object new {}

We can create [JsonProperty("name")] attributes for proper JSON naming, but the best variant is to create setting variable:
var settings = new JsonSerializerSettings()
            {   
                Ignore nulls:
                NullValueHandling = NullValueHandling.Ignore,
                //To chose naming convention
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                //JSON formatting
                Formatting = Formatting.Indented
            };

and then pass the settings variable to JsonConvert.SerializeObject(productsInRange,settings);

For Problem 7 comment NullValueHandling = NullValueHandling.Ignore in  SerializeObjectWithJsonSettings(object obj) method
For Problem 8 uncomment it
*/