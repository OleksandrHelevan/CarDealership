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
                // Allow placing an order even if not in stock (preorder)
                // If product is available, we will decrement stock; otherwise, keep amount as is

                var client = ctx.Clients.FirstOrDefault(c => c.Id == buyCarDto.ClientId);
                if (client == null)
                    throw new InvalidOperationException($"РљР»С–С”РЅС‚Р° РЅРµ Р·РЅР°Р№РґРµРЅРѕ: ID={buyCarDto.ClientId}");

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

                // Decrement amount only if currently in stock and amount > 0
                if (product.InStock && product.Amount > 0)
                {
                    // DB trigger expected to update in_stock accordingly
                    product.Amount = Math.Max(0, product.Amount - 1);
                    ctx.Products.Update(product);
                }

                ctx.SaveChanges();
                tx.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var msg = ex.InnerException?.Message ?? ex.Message;
                MessageBox.Show($"РџРѕРјРёР»РєР° РїРѕРєСѓРїРєРё: {msg}", "РџРѕРјРёР»РєР°", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private string GenerateProductNumber()
        {
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}


