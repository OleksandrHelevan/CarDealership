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
                System.Diagnostics.Debug.WriteLine($"PaymentType: {buyCarDto.PaymentType}, Delivery: {buyCarDto.Delivery}");

                var product = _productRepository.GetById(buyCarDto.Id);
                if (product == null)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: Product not found: {buyCarDto.Id}");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"Product found: ID={product.Id}, Number={product.Number}, InStock={product.InStock}");
                
                if (product.GasolineCarId != null)
                    System.Diagnostics.Debug.WriteLine($"GasolineCarId: {product.GasolineCarId}");
                if (product.ElectroCarId != null)
                    System.Diagnostics.Debug.WriteLine($"ElectroCarId: {product.ElectroCarId}");
                
                var client = _clientRepository.GetById(buyCarDto.ClientId);
                if (client == null)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: Client not found: {buyCarDto.ClientId}");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"Client found: ID={client.Id}, Name={client.PassportData.FirstName} {client.PassportData.LastName}");

                try
                {
                    var orderDto = new OrderDto
                    {
                        Client = ClientMapper.ToDto(client),
                        ClientId = client.Id,
                        Product = ProductMapper.ToDto(product),
                        ProductId = product.Id,
                        OrderDate = DateTime.Now,
                        PaymentType = buyCarDto.PaymentType,
                        Delivery = buyCarDto.Delivery
                    };


                    System.Diagnostics.Debug.WriteLine($"OrderDto created: Client={orderDto.Client.PassportData.FirstName}, Product={orderDto.Product.Number}, PaymentType={orderDto.PaymentType}, Delivery={orderDto.Delivery}");
                    
                    System.Diagnostics.Debug.WriteLine($"OrderDto created: Client={orderDto.Client.PassportData.FirstName}, Product={orderDto.Product.Number}");

                    // Use order service to add the order
                    System.Diagnostics.Debug.WriteLine("Calling OrderService.Add...");
                    _orderService.Add(orderDto);
                    
                    System.Diagnostics.Debug.WriteLine("Order created successfully");
                    
                    return true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR creating order: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    throw; // Rethrow to outer catch
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"BuyCar ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                return false;
            }
        }


        private string GenerateProductNumber()
        {
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}
