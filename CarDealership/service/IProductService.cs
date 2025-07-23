using CarDealership.entity;
using CarDealership.model;


namespace CarDealership.service
{
    public interface IProductService
    {
        List<ProductDto> GetAll();
        ProductDto? GetById(int id);
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(int id);
        bool ExistsByNumber(string number);
    }
}