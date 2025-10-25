using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CarDealership.entity;

[Table("payment_history")]
public class PaymentHistory
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
    [Column("amount", TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    [Column("card_last4")]
    [MaxLength(8)]
    public string CardLast4 { get; set; } = null!;

    [Required]
    [Column("created_at", TypeName = "timestamptz")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Required]
    [Column("content_type")]
    [MaxLength(100)]
    public string ContentType { get; set; } = "application/pdf";

    [Required]
    [Column("receipt_pdf")]
    public byte[] ReceiptPdf { get; set; } = Array.Empty<byte>();
}

