using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CarDealership.enums;

namespace CarDealership.entity;

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

    public Client Client { get; set; } = null!;

    [Required]
    [ForeignKey(nameof(Product))]
    [Column("product_id")]
    public int ProductId { get; set; }

    public Product Product { get; set; } = null!;

    [Required]
    [Column("order_date", TypeName = "timestamp")]
    public DateTime OrderDate { get; set; }

    [Required]
    [Column("payment_type")]
    public PaymentType PaymentType { get; set; }

    [Required]
    [Column("delivery")]
    public bool Delivery { get; set; }
}