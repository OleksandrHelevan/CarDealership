using CarDealership.dto;
using CarDealership.enums;

namespace CarDealership.dto;

public class OrderDto
{
    public int Id { get; set; }
    public int ClientId { get; set; }
    public ClientDto Client { get; set; }
    public ProductDto Product { get; set; }
    public DateTime OrderDate { get; set; }
    public PaymentType PaymentType { get; set; }
    public bool Delivery { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string CarName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int OrderId { get; set; }
}