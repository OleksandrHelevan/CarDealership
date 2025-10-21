using CarDealership.enums;

namespace CarDealership.dto;

public class ProductDto
{
    public int Id { get; set; }
    public string Number { get; set; }
 
    public string CountryOfOrigin { get; set; }

    public bool InStock { get; set; }

    public DateTime? AvailableFrom { get; set; }
    
    public Vehicle Vehicle { get; set; }

    public CarType CarType => Vehicle?.CarType ?? throw new InvalidOperationException("Vehicle is null");

    public ProductDto(int id, string number, string countryOfOrigin, bool inStock, DateTime? availableFrom, Vehicle vehicle)
    {
        Id = id;
        Number = number;
        CountryOfOrigin = countryOfOrigin;
        InStock = inStock;
        AvailableFrom = availableFrom;
        Vehicle = vehicle;
    }
    
}
