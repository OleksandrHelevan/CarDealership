using CarDealership.entity;


namespace CarDealership.service
{
    public interface IProductService
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        bool Create(Product product);
        bool Update(Product product);
        bool Delete(int id);
        bool ExistsByNumber(string number);
    }
}