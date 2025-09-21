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
            _context.Orders.Add(order);
            _context.SaveChanges();
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