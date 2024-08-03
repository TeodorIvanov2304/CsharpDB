using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using TravelAgency.Data;
using TravelAgency.Data.Models;
using TravelAgency.DataProcessor.ImportDtos;
using TravelAgency.Utilities;

namespace TravelAgency.DataProcessor
{
    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data format!";
        private const string DuplicationDataMessage = "Error! Data duplicated.";
        private const string SuccessfullyImportedCustomer = "Successfully imported customer - {0}";
        private const string SuccessfullyImportedBooking = "Successfully imported booking. TourPackage: {0}, Date: {1}";

        public static string ImportCustomers(TravelAgencyContext context, string xmlString)
        {
            StringBuilder sb = new();
            XmlHelper xmlHelper = new XmlHelper();
            const string root = "Customers";

            ImportCustomerDto[] customerDtos = xmlHelper.Deserialize<ImportCustomerDto[]>(xmlString, root);
            ICollection<Customer> customersToImport = new HashSet<Customer>();

            foreach (var customerDto in customerDtos)
            {
                if (!IsValid(customerDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Customer newCustmer = new Customer() 
                {
                    PhoneNumber = customerDto.PhoneNumber,
                    FullName = customerDto.FullName,
                    Email = customerDto.Email
                };

                if (customersToImport.Any(c=>c.FullName == customerDto.FullName)||
                    customersToImport.Any(c => c.Email == customerDto.Email)||
                    customersToImport.Any(c => c.PhoneNumber == customerDto.PhoneNumber))
                {
                    sb.AppendLine(DuplicationDataMessage);
                    continue;
                }

                customersToImport.Add(newCustmer);
                sb.AppendLine(String.Format(SuccessfullyImportedCustomer, newCustmer.FullName));
            }

            context.Customers.AddRange(customersToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static string ImportBookings(TravelAgencyContext context, string jsonString)
        {
            StringBuilder sb = new();
            ImportBookingDto[] bookingDtos = JsonConvert.DeserializeObject<ImportBookingDto[]>(jsonString)!;
            ICollection<Booking> bookingsToImport = new HashSet<Booking>();


            foreach (var bookingDto in bookingDtos)
            {
                if (!IsValid(bookingDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                DateTime bookingDate;
                bool isValidDate = DateTime.TryParseExact(bookingDto.BookingDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out bookingDate);

                if (!isValidDate)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Booking newBooking = new Booking() 
                {
                    BookingDate = DateTime.ParseExact(bookingDto.BookingDate, "yyyy-MM-dd",CultureInfo.InvariantCulture),
                    Customer = context.Customers.First(c=>c.FullName == bookingDto.CustomerName),
                    TourPackage = context.TourPackages.First(t=>t.PackageName == bookingDto.TourPackageName),
                };

                bookingsToImport.Add(newBooking);
                sb.AppendLine(String.Format(SuccessfullyImportedBooking, bookingDto.TourPackageName, bookingDto.BookingDate.ToString()));
            }

            context.Bookings.AddRange(bookingsToImport);
            context.SaveChanges();
            return sb.ToString().TrimEnd();
        }

        public static bool IsValid(object dto)
        {
            var validateContext = new ValidationContext(dto);
            var validationResults = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(dto, validateContext, validationResults, true);

            foreach (var validationResult in validationResults)
            {
                string currValidationMessage = validationResult.ErrorMessage;
            }

            return isValid;
        }
    }
}
