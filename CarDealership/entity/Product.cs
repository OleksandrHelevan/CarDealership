using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("number")]
        public string Number { get; set; } = null!;

        [Required]
        [Column("country_of_origin")]
        public string CountryOfOrigin { get; set; } = null!;

        [Required]
        [Column("in_stock")]
        public bool InStock { get; set; }

        [Column("available_from")]
        public DateTime? AvailableFrom { get; set; }
        
        [Required]
        [Column("car_type")]
        public CarType CarType { get; set; }
        
        [ForeignKey("electro_car_id")]
        public ElectroCar? ElectroCar { get; set; }
        
        [ForeignKey("gasoline_car_id")]
        public GasolineCar? GasolineCar { get; set; }
    }
}