using CarDealership.dto;

namespace CarDealership.service
{
    public interface IBuyService
    {
        bool BuyCar(BuyCarDto buyCarDto);
    }
}
