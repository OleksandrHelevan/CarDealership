using CarDealership.dto;
using CarDealership.entity;
using CarDealership.enums;
using CarDealership.mapper;
using CarDealership.repo;
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
                // Get the client
                var client = _clientRepository.GetById(buyCarDto.ClientId);
                if (client == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Client not found with ID: {buyCarDto.ClientId}");
                    return false;
                }

                // Generate unique product number
                string productNumber = GenerateProductNumber();
                
                // Create product entity
                var product = new Product
                {
                    Number = productNumber,
                    CountryOfOrigin = buyCarDto.CountryOfOrigin,
                    InStock = true,
                    AvailableFrom = buyCarDto.AvailableFrom ?? DateTime.Now
                };

                // Set the appropriate foreign key ID based on car type
                if (buyCarDto.CarType == CarType.Gasoline)
                {
                    product.GasolineCarId = buyCarDto.CarId;
                }
                else if (buyCarDto.CarType == CarType.Electro)
                {
                    product.ElectroCarId = buyCarDto.CarId;
                }

                // Save product to database
                _productRepository.Add(product);

                // Create order DTO
                var orderDto = new OrderDto
                {
                    Client = ClientMapper.ToDto(client),
                    Product = ProductMapper.ToDto(product),
                    OrderDate = DateTime.Now,
                    PaymentType = buyCarDto.PaymentType,
                    Delivery = buyCarDto.Delivery
                };

                // Create order
                _orderService.Add(orderDto);
                
                return true;
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
