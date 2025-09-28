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
                System.Diagnostics.Debug.WriteLine($"Adding order: ClientId={order.ClientId}, ProductId={order.ProductId}");
                
                // Make sure Client and Product are properly set
                if (order.Client == null)
                {
                    var client = _context.Clients.Find(order.ClientId);
                    if (client != null)
                    {
                        order.Client = client;
                        System.Diagnostics.Debug.WriteLine($"Client loaded: {client.PassportData.FirstName} {client.PassportData.LastName}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"WARNING: Client with ID {order.ClientId} not found");
                    }
                }
                
                if (order.Product == null)
                {
                    var product = _context.Products.Find(order.ProductId);
                    if (product != null)
                    {
                        order.Product = product;
                        System.Diagnostics.Debug.WriteLine($"Product loaded: {product.Number}");
                    }
                    else
                    {
                        System.Diagnostics.Debug.WriteLine($"WARNING: Product with ID {order.ProductId} not found");
                    }
                }
                
                // Create a new order with only the necessary properties
                var newOrder = new Order
                {
                    ClientId = order.ClientId,
                    ProductId = order.ProductId,
                    OrderDate = order.OrderDate,
                    PaymentType = order.PaymentType,
                    Delivery = order.Delivery,
                };
                
                _context.Orders.Add(newOrder);
                var changes = _context.SaveChanges();
                
                // Copy the generated ID back to the original order
                order.Id = newOrder.Id;
                
                System.Diagnostics.Debug.WriteLine($"SaveChanges result: {changes} changes saved");
                System.Diagnostics.Debug.WriteLine($"Order ID after save: {order.Id}");
                System.Diagnostics.Debug.WriteLine("=== OrderRepositoryImpl.Add END ===");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR in OrderRepositoryImpl.Add: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
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