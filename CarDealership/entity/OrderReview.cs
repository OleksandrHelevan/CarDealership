using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

[Table("order_reviews")]
public class OrderReview
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [ForeignKey(nameof(Order))]
    [Column("order_id")]
    public int OrderId { get; set; }

    public Order Order { get; set; } = null!;

    [Required]
    [Column("status")]
    public RequestStatus Status { get; set; } = RequestStatus.Pending;

    [Column("message")]
    [MaxLength(500)]
    public string? Message { get; set; }

    // Flags derived from Order at creation time
    [Required]
    [Column("requires_delivery_address")]
    public bool RequiresDeliveryAddress { get; set; }

    [Required]
    [Column("requires_card_number")]
    public bool RequiresCardNumber { get; set; }

    [Column("card_number")]
    [MaxLength(32)]
    public string? CardNumber { get; set; }

    [Required]
    [Column("created_at", TypeName = "timestamptz")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at", TypeName = "timestamptz")]
    public DateTime? UpdatedAt { get; set; }
}
