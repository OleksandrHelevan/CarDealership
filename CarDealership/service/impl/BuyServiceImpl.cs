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
                
                // Use a single database context for the entire transaction
                using var context = new DealershipContext();
                
                // Test database connection
                try
                {
                    var testCount = context.Clients.Count();
                    System.Diagnostics.Debug.WriteLine($"Database connection OK. Total clients: {testCount}");
                }
                catch (Exception dbEx)
                {
                    System.Diagnostics.Debug.WriteLine($"Database connection failed: {dbEx.Message}");
                    return false;
                }
                
                // Get the client
                var client = context.Clients.FirstOrDefault(c => c.Id == buyCarDto.ClientId);
                if (client == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Client not found with ID: {buyCarDto.ClientId}");
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"Client found: {client.Id}");

                // Find existing product by car ID and type
                Product existingProduct = null;

                if (buyCarDto.CarType == CarType.Gasoline)
                {
                    existingProduct = context.Products.FirstOrDefault(p => p.GasolineCarId == buyCarDto.Id);
                    System.Diagnostics.Debug.WriteLine($"Looking for gasoline product with GasolineCarId: {buyCarDto.Id}");
                }
                else if (buyCarDto.CarType == CarType.Electro)
                {
                    existingProduct = context.Products.FirstOrDefault(p => p.ElectroCarId == buyCarDto.Id);
                    System.Diagnostics.Debug.WriteLine($"Looking for electro product with ElectroCarId: {buyCarDto.Id}");
                }

                if (existingProduct == null)
                {
                    System.Diagnostics.Debug.WriteLine($"Product not found for car ID: {buyCarDto.Id}");
                    // Let's see what products exist
                    var allProducts = context.Products.ToList();
                    System.Diagnostics.Debug.WriteLine($"Total products in database: {allProducts.Count}");
                    foreach (var p in allProducts)
                    {
                        System.Diagnostics.Debug.WriteLine($"Product ID: {p.Id}, GasolineCarId: {p.GasolineCarId}, ElectroCarId: {p.ElectroCarId}");
                    }
                    return false;
                }
                System.Diagnostics.Debug.WriteLine($"Product found: {existingProduct.Id}");

                // Create order entity directly
                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    PaymentType = buyCarDto.PaymentType,
                    Delivery = buyCarDto.Delivery,
                    ClientId = client.Id,
                    ProductId = existingProduct.Id,
                    Client = client,
                    Product = existingProduct
                };

                System.Diagnostics.Debug.WriteLine($"Order created: Date={order.OrderDate}, PaymentType={order.PaymentType}, Delivery={order.Delivery}");

                // Add order to context and save
                context.Orders.Add(order);
                System.Diagnostics.Debug.WriteLine($"Order added to context");
                
                var changes = context.SaveChanges();
                System.Diagnostics.Debug.WriteLine($"SaveChanges returned: {changes}");
                
                // Verify the order was saved
                var savedOrder = context.Orders.FirstOrDefault(o => o.Client.Id == client.Id && o.Product.Id == existingProduct.Id);
                if (savedOrder != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Order verified in database with ID: {savedOrder.Id}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR: Order not found in database after save!");
                }
                
                System.Diagnostics.Debug.WriteLine($"=== BUY CAR SUCCESS ===");
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"=== BUY CAR ERROR ===");
                System.Diagnostics.Debug.WriteLine($"BuyCar error: {ex.Message}");
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
