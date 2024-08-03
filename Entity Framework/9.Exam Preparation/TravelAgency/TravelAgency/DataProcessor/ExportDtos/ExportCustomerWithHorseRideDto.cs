namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomerWithHorseRideDto
    {
        public string  FullName { get; set; }
        public string PhoneNumber { get; set; }

        public ExportCustomersWithHorseBooking[] Bookings { get; set; }
    }
}
