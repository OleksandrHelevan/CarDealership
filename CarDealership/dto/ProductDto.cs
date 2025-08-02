using CarDealership.enums;

namespace CarDealership.dto;

public class ProductDto
{
    public string Number { get; set; }
 
    public string CountryOfOrigin { get; set; }

    public bool InStock { get; set; }

    public DateTime? AvailableFrom { get; set; }
    
    public CarType CarType { get; set; }
        
    public CarDto Car { get; set; }

    public ProductDto(string number, string countryOfOrigin, bool inStock, DateTime? availableFrom, CarType carType, CarDto car)
    {
        Number = number;
        CountryOfOrigin = countryOfOrigin;
        InStock = inStock;
        AvailableFrom = availableFrom;
        CarType = carType;
        Car = car;
        
    }
    
}