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
            try
            {
                System.Diagnostics.Debug.WriteLine("=== OrderRepositoryImpl.Add START ===");
                System.Diagnostics.Debug.WriteLine(
                    $"Adding order: ClientId={order.ClientId}, ProductId={order.ProductId}");

                // Завантажуємо повного клієнта
                var client = _context.Clients
                    .Include(c => c.PassportData)
                    .Include(c => c.User)
                    .FirstOrDefault(c => c.Id == order.ClientId);

                if (client == null)
                {
                    throw new Exception($"Client with ID {order.ClientId} not found");
                }

                System.Diagnostics.Debug.WriteLine(
                    $"Client loaded: {client.PassportData.FirstName} {client.PassportData.LastName}");

                var product = _context.Products
                    .Include(p => p.Car)
                    .ThenInclude(c => (c as GasolineCar).Engine)
                    .Include(p => p.Car)
                    .ThenInclude(c => (c as ElectroCar).Engine)
                    .FirstOrDefault(p => p.Id == order.ProductId);

                if (product == null)
                {
                    throw new Exception($"Product with ID {order.ProductId} not found");
                }

                System.Diagnostics.Debug.WriteLine($"Product loaded: {product.ProductNumber}");

                // Створюємо новий об’єкт Order з навігаційними властивостями
                var newOrder = new Order
                {
                    Client = client,
                    ClientId = client.Id,
                    Product = product,
                    ProductId = product.Id,
                    OrderDate = order.OrderDate,
                    PaymentType = order.PaymentType,
                    DeliveryRequired = order.DeliveryRequired,
                };

                _context.Orders.Add(newOrder);
                var changes = _context.SaveChanges();

                // Копіюємо згенерований ID назад у оригінальний об’єкт
                order.Id = newOrder.Id;

                System.Diagnostics.Debug.WriteLine($"SaveChanges result: {changes} changes saved");
                System.Diagnostics.Debug.WriteLine($"Order ID after save: {order.Id}");
                System.Diagnostics.Debug.WriteLine("=== OrderRepositoryImpl.Add END ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in OrderRepositoryImpl.Add: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                throw; // Пробросимо далі, щоб сервіс міг відреагувати
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
            return _context.Orders.ToList();
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