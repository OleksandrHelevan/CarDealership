using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("products")]
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required] [Column("number")] public string Number { get; set; } = null!;

        [Required]
        [Column("country_of_origin")]
        public string CountryOfOrigin { get; set; } = null!;

        [Required] [Column("in_stock")] public bool InStock { get; set; }

        [Column("available_from")] public DateTime? AvailableFrom { get; set; }

        [NotMapped] public object? Vehicle => (object?)ElectroCar ?? GasolineCar;

        [Column("electro_car_id")] public int? ElectroCarId { get; set; }

        [Column("gasoline_car_id")] public int? GasolineCarId { get; set; }

        public ElectroCar? ElectroCar { get; set; }

        public GasolineCar? GasolineCar { get; set; }
    }

}