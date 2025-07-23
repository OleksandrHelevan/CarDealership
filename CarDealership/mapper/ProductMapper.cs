using CarDealership.entity;
using CarDealership.model;

namespace CarDealership.mapper;

public class ProductMapper
{
    public static ProductDto ToDto(Product e)
    {
        CarDto car = new CarDto();
        if (e.ElectroCar != null)
            car = ElectroCarMapper.ToDto(e.ElectroCar);
        if (e.GasolineCar != null)
            car = GasolineCarMapper.ToDto(e.GasolineCar);

        return new ProductDto(e.Number, e.CountryOfOrigin, e.InStock, e.AvailableFrom, e.CarType, car);
    }
}
