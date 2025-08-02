using CarDealership.enums;

namespace CarDealership.dto;

public class OrderDto
{
    public ClientDto Client { get; set; }
    
    public ProductDto Product { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public PaymentType PaymentType { get; set; }
    
    public bool Delivery {get; set;}
}