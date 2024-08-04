namespace TravelAgency.DataProcessor.ExportDtos
{
    public class ExportCustomerWithHorseRideDto
    {
        public string FullName { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public ExportCustomersWithHorseBooking[] Bookings { get; set; } = new ExportCustomersWithHorseBooking[0];
    }
}
