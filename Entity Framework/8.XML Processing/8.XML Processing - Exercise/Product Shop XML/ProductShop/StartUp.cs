using AutoMapper;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.DTOs.Import;
using ProductShop.Models;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            using ProductShopContext context = new ProductShopContext();

            //1
            //string userString = File.ReadAllText("../../../Datasets/users.xml");
            //Console.WriteLine(ImportUsers(context, userString));

            //2
            //string productString = File.ReadAllText("../../../Datasets/products.xml");
            //Console.WriteLine(ImportProducts(context, productString));

            //3
            //string categoryString = File.ReadAllText("../../../Datasets/categories.xml");
            //Console.WriteLine(ImportCategories(context, categoryString));

            //4
            //string categoryProductString = File.ReadAllText("../../../Datasets/categories-products.xml");
            //Console.WriteLine(ImportCategoryProducts(context, categoryProductString));

            //5
            Console.WriteLine(GetProductsInRange(context));

            //6
            //Console.WriteLine(GetSoldProducts(context));

            //7
            //Console.WriteLine(GetCategoriesByProductsCount(context));

            //8
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        //1. Import Users
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UserImportDTO[]), new XmlRootAttribute("Users"));
            using StringReader reader = new StringReader(inputXml);

            UserImportDTO[] importUserDtos;

            importUserDtos = (UserImportDTO[])serializer.Deserialize(reader);

            var users = importUserDtos.Select(dto => new User()
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Age = dto.Age,
            })
            .ToArray();


            context.Users.AddRange(users);
            context.SaveChanges();
            return $"Successfully imported {users.Length}";
        }


        //2. Import Products
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ImportProductDto[]), new XmlRootAttribute("Products"));
            using StringReader reader = new StringReader(inputXml);

            var productsImportDto = (ImportProductDto[])serializer.Deserialize(reader);

            Product[] validProducts = productsImportDto.Select(dto => new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
                SellerId = dto.SellerId,
                BuyerId = dto.BuyerId
            })
            .ToArray();

            context.Products.AddRange(validProducts);
            context.SaveChanges();

            return $"Successfully imported {validProducts.Length}";
        }

        //3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryImportDto[]), new XmlRootAttribute("Categories"));

            using StringReader reader = new StringReader(inputXml);

            CategoryImportDto[]? categoriesImportGtos = (CategoryImportDto[])serializer.Deserialize(reader);

            Category[] categories = categoriesImportGtos
                .Select(dto => new Category()
                {
                    Name = dto.Name,
                })
                .Where(dto => dto.Name != null)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(CategoryProductsImportDto[]), new XmlRootAttribute("CategoryProducts"));

            using StringReader reader = new StringReader(inputXml);

            var categoriesProductsImportDtos = (CategoryProductsImportDto[])serializer.Deserialize(reader);
            int[] categoryIds = context.Categories.Select(dto => dto.Id).ToArray();
            int[] prodctIds = context.Products.Select(dto => dto.Id).ToArray();
            List<CategoryProduct> categoryProducts = new List<CategoryProduct>();

            foreach (var dto in categoriesProductsImportDtos)
            {
                if (prodctIds.Contains(dto.ProductId) && categoryIds.Contains(dto.CategoryId))
                {
                    var categoryProduct = new CategoryProduct()
                    {
                        CategoryId = dto.CategoryId,
                        ProductId = dto.ProductId,
                    };
                    categoryProducts.Add(categoryProduct);
                }
            }

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }


        //5. Export Products In Range
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new ProductsInRangeExportDto()
                {
                    Name = p.Name,
                    Price = p.Price,
                    Buyer = p.Buyer.FirstName != null ? $"{p.Buyer.FirstName} {p.Buyer.LastName}" : " "
                })               
                .Take(10)
                .ToArray();

            return SerializeToXml(productsInRange, "Products");
        }

        //6. Export Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var soldProducts = context.Users
               .Where(u => u.ProductsSold.Any(p => p.BuyerId > 0))
               .Select(u => new SellerWithProductsExoportDto()
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   SoldProducts = u.ProductsSold.Select(p => new SoldProductsExportDto()
                   {
                       Name = p.Name,
                       Price = p.Price,

                   })
                   .ToList()
               })
               .OrderBy(p => p.LastName)
               .ThenBy(p => p.FirstName)
               .Take(5)
               .ToList();
            return SerializeToXml(soldProducts,"Users");
        }

        //7. Export Categories By Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var allCategories = context.Categories.Select(dto => new CategoriesWithProductsExportDto()
            {
                Name = dto.Name,
                Count = dto.CategoryProducts.Count(),
                AveragePrice = dto.CategoryProducts.Average(p => p.Product.Price),
                TotalRevenue = dto.CategoryProducts.Sum(p => p.Product.Price)
            })
            .OrderByDescending(dto=>dto.Count)
            .ThenBy(dto=>dto.TotalRevenue)
            .ToArray();
            return SerializeToXml(allCategories,"Categories");
        }

        //8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var totalUsersCount = context.Users.Count(u => u.ProductsSold.Any());

            var users = context.Users
                .Where(u => u.ProductsSold.Any())
                .OrderByDescending(u => u.ProductsSold.Count)
                .Select(u => new UserExportDto()
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new SoldProductsDto
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold
                            .Select(p => new ProductExportDto
                            {
                                Name = p.Name,
                                Price = p.Price
                            })
                            .OrderByDescending(p => p.Price)
                            .ToList()
                    }
                })
                .Take(10)
                .ToList();

            var result = new UsersExportDto
            {
                Count = totalUsersCount,
                Users = users
            };

            return SerializeToXml(result, "Users");
        }

        //Serializing method!!!
        private static string SerializeToXml<T>(T dto, string xmlRootAttribute, bool omitDeclaration = false)
        {
            XmlSerializer xmlSerializer =
                new XmlSerializer(typeof(T), new XmlRootAttribute(xmlRootAttribute));

            StringBuilder stringBuilder = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);

            var settings = new XmlWriterSettings()
            {
                OmitXmlDeclaration = omitDeclaration,
                Encoding = new UTF8Encoding(false),
                Indent = true,
            };

            XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
            xmlSerializerNamespaces.Add(string.Empty, string.Empty);

            try
            {
                xmlSerializer.Serialize(stringWriter, dto, xmlSerializerNamespaces);
            }
            catch (Exception)
            {

                throw new ArgumentException("Error with serialization");
            }


            return stringBuilder.ToString();
        }
    }
}