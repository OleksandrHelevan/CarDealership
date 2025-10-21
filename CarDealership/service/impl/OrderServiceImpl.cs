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
            try
            {
                System.Diagnostics.Debug.WriteLine("=== OrderService.Add START ===");
                System.Diagnostics.Debug.WriteLine($"OrderDto: ClientId={orderDto.ClientId}, Product={orderDto.Product?.Number}");

                var order = OrderMapper.ToEntity(orderDto);
                System.Diagnostics.Debug.WriteLine($"Order entity created: ClientId={order.ClientId}, ProductId={order.ProductId}");

                _orderRepository.Add(order);
                System.Diagnostics.Debug.WriteLine($"Order added to repository, ID={order.Id}");
                System.Diagnostics.Debug.WriteLine("=== OrderService.Add END ===");
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                System.Diagnostics.Debug.WriteLine($"ERROR in OrderService.Add: {msg}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                System.Windows.MessageBox.Show($"Помилка створення замовлення: {msg}", "Помилка", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                throw; // Rethrow to let caller handle
            }
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
