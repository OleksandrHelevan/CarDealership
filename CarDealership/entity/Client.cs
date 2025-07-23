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

        [Required]
        [ForeignKey("keys")]
        [Column("user_id")]
        public virtual User User { get; set; }

        [Required]
        [ForeignKey("PassportData")]
        [Column("passport_data_id")]
        public virtual PassportData PassportData { get; set; }
    }
}