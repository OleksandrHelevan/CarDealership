using CarDealership.enums;

namespace CarDealership.dto;

public class ProductDto
{
    public string Number { get; set; }
 
    public string CountryOfOrigin { get; set; }

    public bool InStock { get; set; }

    public DateTime? AvailableFrom { get; set; }
    
    public Vehicle Vehicle { get; set; }

    // Computed property to determine car type based on Vehicle type
    public CarType CarType => Vehicle switch
    {
        GasolineCarDto => CarType.Gasoline,
        ElectroCarDto => CarType.Electro,
        _ => throw new InvalidOperationException("Unknown vehicle type")
    };

    public ProductDto(string number, string countryOfOrigin, bool inStock, DateTime? availableFrom, Vehicle vehicle)
    {
        Number = number;
        CountryOfOrigin = countryOfOrigin;
        InStock = inStock;
        AvailableFrom = availableFrom;
        Vehicle = vehicle;
    }
    
}