using CarDealership.config;
using CarDealership.dto;
using CarDealership.entity;
using CarDealership.repo;
using System.Windows;

namespace CarDealership.service.impl
{
    public class BuyServiceImpl : IBuyService
    {
        private readonly IProductRepository _productRepository;
        private readonly IOrderService _orderService;
        private readonly IClientRepository _clientRepository;

        public BuyServiceImpl(IProductRepository productRepository, IOrderService orderService, IClientRepository clientRepository)
        {
            _productRepository = productRepository;
            _orderService = orderService;
            _clientRepository = clientRepository;
        }

        public bool BuyCar(BuyCarDto buyCarDto)
        {
            try
            {
                using var ctx = new DealershipContext();
                using var tx = ctx.Database.BeginTransaction();

                var product = ctx.Products.FirstOrDefault(p => p.Id == buyCarDto.Id);
                if (product == null)
                    throw new InvalidOperationException($"Product not found: ID={buyCarDto.Id}");

                if (product.Amount <= 0 || !product.InStock)
                    throw new InvalidOperationException($"Продукт №{product.Number} відсутній на складі.");

                var client = ctx.Clients.FirstOrDefault(c => c.Id == buyCarDto.ClientId);
                if (client == null)
                    throw new InvalidOperationException($"Клієнта не знайдено: ID={buyCarDto.ClientId}");

                var order = new Order
                {
                    ClientId = client.Id,
                    ProductId = product.Id,
                    OrderDate = DateTime.UtcNow,
                    PaymentType = buyCarDto.PaymentType,
                    Delivery = buyCarDto.Delivery,
                    Address = buyCarDto.Delivery ? buyCarDto.Address : null,
                    PhoneNumber = string.IsNullOrWhiteSpace(buyCarDto.PhoneNumber) ? null : buyCarDto.PhoneNumber
                };

                ctx.Orders.Add(order);

                // Decrement amount; DB trigger updates in_stock accordingly
                product.Amount = Math.Max(0, product.Amount - 1);
                ctx.Products.Update(product);

                ctx.SaveChanges();
                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"Помилка покупки: {msg}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private string GenerateProductNumber()
        {
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}

