using CarDealership.model;

namespace CarDealership.service
{
    public interface IOrderService
    {
        void Add(OrderDto orderDto);
        void Update(OrderDto orderDto, int id);
        void Delete(int id);
        OrderDto GetById(int id);
        List<OrderDto> GetAll();
    }
}