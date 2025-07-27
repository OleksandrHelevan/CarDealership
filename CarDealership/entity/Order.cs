using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity
{
    [Table("orders")]
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [ForeignKey("client_id")]
        public virtual Client Client { get; set; }

        [Required]
        [ForeignKey("product_id")]
        public virtual Product Product { get; set; }

        [Required]
        [Column("order_date")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column("payment_type")]
        public PaymentType PaymentType { get; set; }

        [Required]
        [Column("delivery")]
        public bool Delivery { get; set; }
    }
}