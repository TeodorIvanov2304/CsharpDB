﻿using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.DTOs.Export;
using ProductShop.Models;
using System.Runtime.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            ProductShopContext context = new ProductShopContext();
            //1
            //string userText = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, userText));
            //2
            //string productText = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, productText));
            //03
            //string categoriesText = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, categoriesText));
            //04
            //string catprodText = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, catprodText));
            //5.
            //Console.WriteLine(GetProductsInRange(context));
            //6
            //Console.WriteLine(GetSoldProducts(context));
            //7
            //Console.WriteLine(GetCategoriesByProductsCount(context));
            //8
            Console.WriteLine(GetUsersWithProducts(context));
        }

        //1.Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {   
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);
            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
        //2.Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {   
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.Products.AddRange(products);
            context.SaveChanges();
            return $"Successfully imported {products.Count}";
        }

        //3. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson);

            categories.RemoveAll(c => c.Name == null);

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }


        //4. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert
                .DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoriesProducts.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }

        //5.Export Products in Range
        public static string GetProductsInRange(ProductShopContext context)
        {   
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

            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            string products = JsonConvert.SerializeObject(productsInRange,settings);

            return products;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            //var soldProducts = context.Users
            //    .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
            //    .Select(u => new
            //    {
            //        u.FirstName,
            //        u.LastName,
            //        SoldProducts = u.ProductsSold.Select(p => new
            //        {
            //            p.Name,
            //            p.Price,
            //            BuyerFirstName = p.Buyer.FirstName,
            //            BuyerLastName = p.Buyer.LastName
            //        })
            //    })
            //    .OrderBy(u => u.LastName)
            //    .ThenBy(u => u.FirstName)
            //    .ToList();

            var soldProducts = context.Users
               .Where(u => u.ProductsSold.Any(p => p.BuyerId != null))
               .Select(u => new SellerWithProductsDto()
               {
                   FirstName = u.FirstName,
                   LastName = u.LastName,
                   SoldProducts = u.ProductsSold.Select(p => new SoldProductsDto()
                   {
                       Name = p.Name,
                       Price = p.Price,
                       BuyerFirstName = p.Buyer.FirstName,
                       BuyerLastName = p.Buyer.LastName
                   })
               })
               .OrderBy(p => p.LastName)
               .ThenBy(p => p.FirstName)
               .ToList();

            return SerializeObjectWithJsonSettings(soldProducts);
        }

        //7. Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.
                Select(c => new
                {
                    Category = c.Name,
                    ProductsCount = c.CategoriesProducts.Count(),
                    AveragePrice = $"{c.CategoriesProducts.Average(p => p.Product.Price):F2}",
                    TotalRevenue = $"{c.CategoriesProducts.Sum(cp => cp.Product.Price):F2}"

                })
                .OrderByDescending(pc => pc.ProductsCount)
                .ToList();
                
            return SerializeObjectWithJsonSettings(categories);
        }

        // 8. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(u => u.ProductsSold.Any(p => p.BuyerId != null && p.Price != null))
                .Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = u.ProductsSold
                        .Where(p => p.BuyerId != null && p.Price != null)
                        .Select(p => new
                        {
                            p.Name,
                            p.Price
                        })
                        .ToArray()
                })
                .OrderByDescending(u => u.SoldProducts.Length)
                .ToArray();

            var output = new
            {
                UsersCount = users.Length,
                Users = users.Select(u => new
                {
                    u.FirstName,
                    u.LastName,
                    u.Age,
                    SoldProducts = new
                    {
                        Count = u.SoldProducts.Length,
                        Products = u.SoldProducts
                    }
                })
            };
            return SerializeObjectWithJsonSettings(output);
        }


        //Serializing method
        private static string SerializeObjectWithJsonSettings(object obj)
        {
            var settings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            return JsonConvert.SerializeObject(obj, settings);
        }
    }
}