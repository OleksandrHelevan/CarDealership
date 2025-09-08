using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity
{
    [Table("clients")]
    public class Client
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // FK на keys (user)
        [Required]
        [Column("user_id")]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; } = null!;

        // FK на passport_data
        [Required]
        [Column("passport_data_id")]
        public int PassportDataId { get; set; }

        [ForeignKey(nameof(PassportDataId))]
        public virtual PassportData PassportData { get; set; } = null!;
    }
}