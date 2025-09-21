using CarDealership.mapper;
using CarDealership.dto;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void Add(OrderDto orderDto)
        {
            var order = OrderMapper.ToEntity(orderDto);
            _orderRepository.Add(order);
        }

        public void Update(OrderDto orderDto, int id)
        {
            var existing = _orderRepository.GetById(id);
            if (existing == null) return;

            var updated = OrderMapper.ToEntity(orderDto);
            updated.Id = id;

            _orderRepository.Update(updated);
        }

        public void Delete(int id)
        {
            var order = _orderRepository.GetById(id);
            if (order != null)
            {
                _orderRepository.Delete(order);
            }
        }

        public OrderDto GetById(int id)
        {
            var order = _orderRepository.GetById(id);
            return order != null ? OrderMapper.ToDto(order) : null;
        }

        public List<OrderDto> GetAll()
        {
            return _orderRepository
                .GetAll()
                .Select(OrderMapper.ToDto)
                .ToList();
        }

        public List<OrderDto> FindOrdersByClientId(int clientId)
        {
            return _orderRepository
                .FindOrdersByClientId(clientId)
                .Select(OrderMapper.ToDto)
                .ToList();
        }
    }
}