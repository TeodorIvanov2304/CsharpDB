using Newtonsoft.Json;
using TravelAgency.Data;
using TravelAgency.DataProcessor.ExportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Serializer
    {
        public static string ExportGuidesWithSpanishLanguageWithAllTheirTourPackages(TravelAgencyContext context)
        {
            ExportGuideWithSpanish[] guides = context.Guides
                .Where(g=>g.Language == Data.Models.Enums.Language.Spanish)
                .Select(g=> new ExportGuideWithSpanish() 
            {
                FullName = g.FullName,
                TourPackages = g.TourPackagesGuides
                .OrderByDescending(tpg=>tpg.TourPackage.Price)
                .ThenBy(tpg=>tpg.TourPackage.PackageName)
                .Select(tpg => new ExportGuideTourPackages() 
                {
                    Name = tpg.TourPackage.PackageName,
                    Description = tpg.TourPackage.Description,
                    Price = tpg.TourPackage.Price
                })
                .ToArray()
            })
            .OrderByDescending(g=>g.TourPackages.Length)
            .ThenBy(g=>g.FullName)
            .ToArray();

            XmlHelper xmlHelper = new XmlHelper();
            const string xmlRoot = "Guides";
            return xmlHelper.Serialize(guides, xmlRoot);
        }

        public static string ExportCustomersThatHaveBookedHorseRidingTourPackage(TravelAgencyContext context)
        {
            ExportCustomerWithHorseRideDto[] customersWithHorse = context.Customers
                .Where(c => c.Bookings.Any(b => b.TourPackage.PackageName == "Horse Riding Tour"))
                .Select(c => new ExportCustomerWithHorseRideDto()
                {
                    FullName = c.FullName,
                    PhoneNumber = c.PhoneNumber,
                    Bookings = c.Bookings
                        .Where(b => b.TourPackage.PackageName == "Horse Riding Tour")
                        .OrderBy(b => b.BookingDate)
                        .Select(b => new ExportCustomersWithHorseBooking()
                        {
                            TourPackageName = b.TourPackage.PackageName,
                            Date = b.BookingDate.ToString("yyyy-MM-dd")
                        })
                        .ToArray()
                })
                .OrderByDescending(c => c.Bookings.Length)
                .ThenBy(c => c.FullName)
                .ToArray();

            return JsonConvert.SerializeObject(customersWithHorse, Formatting.Indented);
        }
    }
}
