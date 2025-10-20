using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        [Required]
        [MaxLength(50)]
        [Column("product_number")]
        public string ProductNumber { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        [Column("country_of_origin")]
        public string CountryOfOrigin { get; set; } = null!;

        [Required]
        [Column("in_stock")]
        public bool InStock { get; set; }

        [Column("available_from")]
        public DateTime? AvailableFrom { get; set; }

        [Column("available_until")]
        public DateTime? AvailableUntil { get; set; }

        [Required]
        [Column("car_id")]
        public int CarId { get; set; }

        [Required]
        [Column("car_type")]
        public CarType CarType { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(CarId))]
        public virtual Car Car { get; set; } = null!;

        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Computed properties
        [NotMapped]
        public string CarDisplayName => Car?.FullName ?? "Unknown Car";

        [NotMapped]
        public bool IsAvailable => InStock && (AvailableFrom == null || AvailableFrom <= DateTime.UtcNow) && 
                                  (AvailableUntil == null || AvailableUntil >= DateTime.UtcNow);
    }
}