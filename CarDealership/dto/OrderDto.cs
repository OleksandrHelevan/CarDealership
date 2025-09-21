using CarDealership.enums;

namespace CarDealership.dto;

public class OrderDto
{
    public int Id { get; set; }
    public ClientDto Client { get; set; }
    
    public ProductDto Product { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public bool Delivery {get; set;}
    
    // Additional properties for UI display
    public string CarName { get; set; }
    public DateTime CreatedAt { get; set; }
    public int OrderId { get; set; }
}