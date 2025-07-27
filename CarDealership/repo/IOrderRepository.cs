using CarDealership.entity;

namespace CarDealership.repo
{
    public interface IOrderRepository
    {
        void Add(Order order);
        void Update(Order order);
        void Delete(Order order);
        Order GetById(int id);
        List<Order> GetAll();
    }
}