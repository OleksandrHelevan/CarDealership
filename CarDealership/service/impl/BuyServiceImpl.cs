using CarDealership.config;
using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.mapper;
using CarDealership.repo;
using CarDealership.repo.impl;
using CarDealership.service;

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
                System.Diagnostics.Debug.WriteLine($"=== BUY CAR START ===");
                System.Diagnostics.Debug.WriteLine($"CarId: {buyCarDto.Id}, CarType: {buyCarDto.CarType}, ClientId: {buyCarDto.ClientId}");

                using var context = new DealershipContext();

                // Get the client by Id
                var client = context.Clients.FirstOrDefault(c => c.Id == buyCarDto.ClientId);
                var product = buyCarDto.CarType == CarType.Gasoline
                    ? context.Products.FirstOrDefault(p => p.GasolineCarId == buyCarDto.Id)
                    : context.Products.FirstOrDefault(p => p.ElectroCarId == buyCarDto.Id);

                if (client == null || product == null)
                    return false;

                var order = new Order
                {
                    Client = client,
                    Product = product,
                    ClientId = client.Id,
                    ProductId = product.Id,
                    OrderDate = DateTime.Now,
                    PaymentType = buyCarDto.PaymentType,
                    Delivery = buyCarDto.Delivery
                };

                context.Orders.Add(order);
                var changes = context.SaveChanges();

                System.Diagnostics.Debug.WriteLine($"SaveChanges returned: {changes}");

                return changes > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BuyCar error: {ex.Message}");
                return false;
            }
        }


        private string GenerateProductNumber()
        {
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}
