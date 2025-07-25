using CarDealership.config;
using CarDealership.entity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace CarDealership.repo.impl
{
    public class ProductRepositoryImpl : IProductRepository
    {
        private readonly DealershipContext _context;

        public ProductRepositoryImpl(DealershipContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
                .Include(p => p.ElectroCar)
                .ThenInclude(e => e.Engine)
                .Include(p => p.GasolineCar)
                .ThenInclude(g => g.Engine)
                .ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products
                .Include(p => p.ElectroCar)
                .ThenInclude(e => e.Engine)
                .Include(p => p.GasolineCar)
                .ThenInclude(g => g.Engine)
                .FirstOrDefault(p => p.Id == id);
        }

        public void Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var product = _context.Products.Find(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();
            }
        }

        public bool ExistsByNumber(string number)
        {
            return _context.Products.Any(p => p.Number == number);
        }
    }
}