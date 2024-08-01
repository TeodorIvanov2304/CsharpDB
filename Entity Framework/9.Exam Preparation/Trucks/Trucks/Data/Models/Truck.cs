using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trucks.Data.Models.Enums;
using static Trucks.Data.DataConstraints;
namespace Trucks.Data.Models
{
    public class Truck
    {
        [Key]
        public int Id { get; set; }

        //Required? It can be ?
        [MaxLength(TruckRegistrationNumberLength)]
        public string RegistrationNumber { get; set; }

        [Required]
        [MaxLength(TruckVinNumberLength)]
        public string VinNumber { get; set; } = null!;

        [MaxLength(TruckTankCapacityMaxLength)]
        public int TankCapacity { get; set; }

        [MaxLength(TruckCargoCapacityMaxLength)]
        public int CargoCapacity { get; set; }

        [Required]
        public CategoryType CategoryType { get; set; }

        [Required]
        public MakeType MakeType { get; set; }

        [Required]
        [ForeignKey(nameof(Despatcher))]
        public int DespatcherId { get; set; }
        public Despatcher Despatcher { get; set; }
        public ICollection<ClientTruck> ClientsTrucks  { get; set; }

    }
}
