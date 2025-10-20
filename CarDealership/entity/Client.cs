using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity
{
    [Table("clients")]
    public class Client
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        [Required]
        [ForeignKey(nameof(PassportData))]
        [Column("passport_data_id")]
        public int PassportDataId { get; set; }

        [ForeignKey(nameof(PassportDataId))]
        public virtual PassportData PassportData { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Computed properties
        [NotMapped]
        public string FullName => $"{PassportData?.FirstName} {PassportData?.LastName}";

        [NotMapped]
        public string DisplayName => $"{FullName} ({User?.Login})";
    }
}