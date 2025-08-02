using CarDealership.entity;
using CarDealership.enums;
using CarDealership.dto;

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

    public static Product ToEntity(ProductDto dto)
    {
        var product = new Product
        {
            Number = dto.Number,
            CountryOfOrigin = dto.CountryOfOrigin,
            InStock = dto.InStock,
            AvailableFrom = dto.AvailableFrom,
            CarType = dto.CarType
        };

        // Залежно від типу — заповнюємо відповідне поле
        if (dto.CarType == CarType.Electro)
        {
            product.ElectroCar = ElectroCarMapper.ToEntity(dto.Car);
        }
        else if (dto.CarType == CarType.Gasoline)
        {
            product.GasolineCar = GasolineCarMapper.ToEntity(dto.Car);
        }

        return product;
    }
}
