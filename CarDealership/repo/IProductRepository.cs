using CarDealership.entity;
using CarDealership.enums;
using System.Collections.Generic;

namespace CarDealership.repo
{
    public interface IProductRepository
    {
        IEnumerable<Product> GetAll();
        Product? GetById(int id);
        void Add(Product product);
        void Update(Product product);
        void Delete(int id);
        bool ExistsByNumber(string number);
        IEnumerable<Product> GetByVehicleIds(List<int> vehicleIds, CarType carType);
    }
}
