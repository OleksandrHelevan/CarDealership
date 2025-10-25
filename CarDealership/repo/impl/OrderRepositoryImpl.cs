using CarDealership.config;
using CarDealership.entity;
using Microsoft.EntityFrameworkCore;

namespace CarDealership.repo.impl
{
    public class OrderRepositoryImpl : IOrderRepository
    {
        private readonly DealershipContext _context;

        public OrderRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            // Minimal FK write: rely on existing IDs; avoids heavy includes
            if (order.ClientId <= 0) throw new ArgumentException("Invalid ClientId");
            if (order.ProductId <= 0) throw new ArgumentException("Invalid ProductId");

            var existsClient = _context.Clients.Any(c => c.Id == order.ClientId);
            if (!existsClient) throw new Exception($"Client with ID {order.ClientId} not found");

            var existsProduct = _context.Products.Any(p => p.Id == order.ProductId);
            if (!existsProduct) throw new Exception($"Product with ID {order.ProductId} not found");

            try
            {
                _context.Orders.Add(new Order
                {
                    ClientId = order.ClientId,
                    ProductId = order.ProductId,
                    OrderDate = order.OrderDate,
                    PaymentType = order.PaymentType,
                    Delivery = order.Delivery
                });

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                throw new InvalidOperationException($"Не вдалося зберегти замовлення: {msg}", ex);
            }
        }


        public void Update(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void Delete(Order order)
        {
            _context.Orders.Remove(order);
            _context.SaveChanges();
        }

        public Order GetById(int id)
        {
            return _context.Orders.FirstOrDefault(o => o.Id == id);
        }

        public List<Order> GetAll()
        {
            return _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Product)
                .ToList();
        }

        public List<Order> FindOrdersByClientId(int clientId)
        {
            using var context = new DealershipContext();

            return context.Orders
                .Include(o => o.Client)
                .Include(o => o.Product)
                .Where(o => o.ClientId == clientId)
                .ToList();
        }
    }
}
