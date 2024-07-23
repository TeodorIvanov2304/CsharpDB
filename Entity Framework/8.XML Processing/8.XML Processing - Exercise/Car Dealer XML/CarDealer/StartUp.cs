using CarDealer.Data;
using CarDealer.DTOs.Export;
using CarDealer.DTOs.Import;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            using CarDealerContext context = new CarDealerContext();

            //9
            //string suppliresXml = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context,suppliresXml));

            //10
            //string partsXml = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsXml));

            //11
            //string carsXml = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsXml));

            //12
            //string customerXml = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customerXml));

            //13
            //string salesXml = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesXml));

            //14
            //Console.WriteLine(GetCarsWithDistance(context));

            //15
            //Console.WriteLine(GetCarsFromMakeBmw(context));

            //16
            //Console.WriteLine(GetLocalSuppliers(context));

            //17
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //18
            //Console.WriteLine(GetTotalSalesByCustomer(context));

            //19
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }

        //9. Import Suppliers
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerilizer = new XmlSerializer(typeof(SupplierImportDTO[]), new XmlRootAttribute("Suppliers"));

            SupplierImportDTO[] importDtos;

            using StringReader reader = new StringReader(inputXml);

            importDtos = (SupplierImportDTO[])xmlSerilizer.Deserialize(reader);

            Supplier[] suppliers = importDtos.Select(dto => new Supplier

            {
                Name = dto.Name,
                IsImporter = dto.IsImporter
            })
            .ToArray();

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();


            return $"Successfully imported {importDtos.Length}";
        }

        //10. Import Parts
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerilizer = new XmlSerializer(typeof(PartsImportDTO[]), new XmlRootAttribute("Parts"));


            using StringReader reader = new StringReader(inputXml);
            PartsImportDTO[] partsImportDTOs;
            partsImportDTOs = (PartsImportDTO[])xmlSerilizer.Deserialize(reader);

            int[] supplierIds = context.Suppliers
                .Select(id => id.Id)
                .ToArray();

            PartsImportDTO[] partsWithValidSupplier = partsImportDTOs
                .Where(p => supplierIds.Contains(p.SupplierId))
                .ToArray();

            Part[] parts = partsWithValidSupplier
                .Select(dto => new Part
                {
                    Name = dto.Name,
                    Price = dto.Price,
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId
                })
                .ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();
            return $"Successfully imported {parts.Length}";
        }

        //11. Import Cars
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlSerializer xmlSerilizer = new XmlSerializer(typeof(CarImportDTO[]), new XmlRootAttribute("Cars"));

            using StringReader reader = new StringReader(inputXml);
            CarImportDTO[] carImportDtos = (CarImportDTO[]?)xmlSerilizer.Deserialize(reader);

            List<Car> cars = new List<Car>();

            foreach (var dto in carImportDtos)
            {
                Car car = new Car()

                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance,

                };

                int[] carPartsId = dto.PartIds
                    .Select(p => p.Id)
                    .Distinct()
                    .ToArray();

                var carParts = new List<PartCar>();

                foreach (var id in carPartsId)
                {
                    carParts.Add(new PartCar
                    {
                        Car = car,
                        PartId = id
                    });
                }
                car.PartsCars = carParts;
                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //12. Import Customers
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            using StringReader reader = new StringReader(inputXml);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CustomerImportDTO[]), new XmlRootAttribute("Customers"));

            CustomerImportDTO[] customerImportDtos = (CustomerImportDTO[])xmlSerializer.Deserialize(reader);

            Customer[] customers = customerImportDtos.Select(dto => new Customer()
            {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                IsYoungDriver = dto.IsYoungDriver
            })
            .ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();
            return $"Successfully imported {customers.Length}";
        }

        //13. Import Sales
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            //Create class SaleImportDTO

            //Create reader
            using StringReader reader = new StringReader(inputXml);

            //Create xml serializer, pass type and root element
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaleImportDTO[]), new XmlRootAttribute("Sales"));

            //Create array of SaleImportDTO and deserialize the xml passing the reader, and casting toSaleImportDTO
            SaleImportDTO[] saleImportDtos = (SaleImportDTO[])xmlSerializer.Deserialize(reader);

            //Create array of valid car ids
            int[] carIds = context.Cars.Select(i => i.Id).ToArray();

            //Create array of sales where CarId is contained in int[] carIds
            Sale[] sales = saleImportDtos.Select(dto => new Sale()
            {
                CarId = dto.CarId,
                CustomerId = dto.CustomerId,
                Discount = dto.Discount
            })
            .Where(s => carIds.Contains(s.CarId))
            .ToArray();

            //Add the sales to the Db
            context.Sales.AddRange(sales);
            //Save changes
            context.SaveChanges();
            return $"Successfully imported {sales.Length}";
        }

        //14. Export Cars With Distance
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var carsWithDistance = context.Cars.Select(c => new CarWithDistanceExportGto() //new DTO
            {
                Make = c.Make,
                Model = c.Model,
                TraveledDistance = c.TraveledDistance
            })
            .OrderBy(c => c.Make)
            .ThenBy(c => c.Model)
            .Take(10)
            .ToArray();

            return SerializeToXml(carsWithDistance, "cars");
        }
        //15. Export Cars from Make BMW
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var carsBmw = context.Cars
                .Where(c=>c.Make == "BMW")
                .Select(dto => new CarBmwExportDto()
                {
                    Id = dto.Id,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance
                })
                .OrderBy(dto => dto.Model)
                .ThenByDescending(dto=>dto.TraveledDistance)
                .ToArray();
            return SerializeToXml(carsBmw,"cars",true);
        }

        //16. Export Local Suppliers
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var localSuppliers = context.Suppliers
                .Where(s=>s.IsImporter == false)
                .Select(dto => new SupplierExportDto()
            {
                Id = dto.Id,
                Name = dto.Name,
                Parts = dto.Parts.Count
            })
            .ToArray();

            return SerializeToXml(localSuppliers,"suppliers");
        }


        //17. Export Cars with Their List of Parts
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var cars =   context.Cars
                .Select(dto => new CarWithPartsExportDto()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TraveledDistance = dto.TraveledDistance,
                    PartsDtos = dto.PartsCars.Select(p=> new CarPartExportDto()
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p=>p.Price)
                    .ToArray()
                })
                .OrderByDescending(dto =>dto.TraveledDistance)
                .ThenBy(dto => dto.Model)
                .Take(5)
                .ToArray();

            return SerializeToXml(cars,"cars");
        }


        //18. Export Total Sales by Customer
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {

            var temp = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count(),
                    SalesInfo = c.Sales.Select(s => new
                    {
                        Prices = c.IsYoungDriver
                        ? s.Car.PartsCars.Sum(pc => Math.Round((double)pc.Part.Price * 0.95, 2))
                        : s.Car.PartsCars.Sum(pc => (double)pc.Part.Price)
                    })
                        .ToArray(),
                })
                .ToArray();

            var customerSalesInfo = temp
                .OrderByDescending(x => x.SalesInfo.Sum(p=>p.Prices))
                .Select(a=> new CustomerWithCarExportDto 
                {
                    FullName = a.FullName,
                    CarsBought = a.BoughtCars,
                    SpentMoney = a.SalesInfo.Sum(b=>(decimal)b.Prices)
                })
                .ToArray();

            /*
            var totalSales = context.Customers
                .Where(c => c.Sales.Any())
                .Select(c => new ExportSalesPerCustomerDTO
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpentMoney = c.Sales.Sum(s =>
                        s.Car.PartsCars.Sum(pc =>
                            Math.Round(c.IsYoungDriver ? pc.Part.Price * 0.95m : pc.Part.Price, 2)
                        )
                    )
                })
                .OrderByDescending(s => s.SpentMoney)
                .ToArray(); 
            */

            return SerializeToXml(customerSalesInfo,"customers");
        }

        //19. Export Sales with Applied Discount
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var sales = context.Sales.Select(dto => new SaleWithDiscount()
            {
                Car = new CarDto()
                {
                    Make = dto.Car.Make,
                    Model = dto.Car.Model,
                    TraveledDistance = dto.Car.TraveledDistance
                },
                Discount = (int)dto.Discount,
                CustomerName = dto.Customer.Name,
                Price = dto.Car.PartsCars.Sum(pc => pc.Part.Price),
                PriceWithDiscount =
                        Math.Round((double)(dto.Car.PartsCars
                            .Sum(p => p.Part.Price) * (1 - (dto.Discount / 100))), 4)
            })
            .ToArray();
            return SerializeToXml(sales,"sales");
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
                Async = true, 
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