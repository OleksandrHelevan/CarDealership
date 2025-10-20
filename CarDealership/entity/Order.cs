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
        [ForeignKey(nameof(Client))]
        [Column("client_id")]
        public int ClientId { get; set; }

        [Required]
        [ForeignKey(nameof(Product))]
        [Column("product_id")]
        public int ProductId { get; set; }

        [Required]
        [Column("order_date")]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        [Column("payment_type")]
        public PaymentType PaymentType { get; set; }

        [Required]
        [Column("delivery_required")]
        public bool DeliveryRequired { get; set; }

        [Column("delivery_address")]
        [MaxLength(500)]
        public string? DeliveryAddress { get; set; }

        [Column("delivery_date")]
        public DateTime? DeliveryDate { get; set; }

        [Column("status")]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column("notes")]
        [MaxLength(1000)]
        public string? Notes { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        [ForeignKey(nameof(ClientId))]
        public virtual Client Client { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public virtual Product Product { get; set; } = null!;

        // Computed properties
        [NotMapped]
        public string OrderNumber => $"ORD-{Id:D6}";

        [NotMapped]
        public decimal TotalAmount => Product?.Car?.Price ?? 0;
        

        [NotMapped]
        public string PaymentTypeString => PaymentType.ToFriendlyString();
    }

    public enum OrderStatus
    {
        Pending,
        Confirmed,
        Processing,
        Shipped,
        Delivered,
        Cancelled,
        Returned
    }
}