using System.Windows.Forms;
using CarDealership.config;
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
                System.Diagnostics.Debug.WriteLine("=== BUY CAR START ===");
                System.Diagnostics.Debug.WriteLine($"CarId: {buyCarDto.Id}, CarType: {buyCarDto.CarType}, ClientId: {buyCarDto.ClientId}");
                System.Diagnostics.Debug.WriteLine($"PaymentType: {buyCarDto.PaymentType}, DeliveryRequired: {buyCarDto.DeliveryRequired}");

                // 1️⃣ Знаходимо продукт
                var product = _productRepository.GetById(buyCarDto.Id);
                if (product == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Product not found: {buyCarDto.Id}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"✅ Product found: ID={product.Id}, Number={product.ProductNumber}, InStock={product.InStock}");

                if (!product.InStock)
                {
                    System.Diagnostics.Debug.WriteLine("⚠️ Product already sold or unavailable.");
                    return false;
                }

                // 2️⃣ Знаходимо клієнта
                var client = _clientRepository.GetById(buyCarDto.ClientId);
                if (client == null)
                {
                    System.Diagnostics.Debug.WriteLine($"❌ Client not found: {buyCarDto.ClientId}");
                    return false;
                }

                System.Diagnostics.Debug.WriteLine($"✅ Client found: ID={client.Id}, Name={client.PassportData.FirstName} {client.PassportData.LastName}");

                // 3️⃣ Формуємо замовлення
                var orderDto = new OrderDto
                {
                    Client = ClientMapper.ToDto(client),
                    ClientId = client.Id,
                    Product = ProductMapper.ToDto(product),
                    ProductId = product.Id,
                    OrderDate = DateTime.UtcNow,
                    PaymentType = buyCarDto.PaymentType,
                    DeliveryRequired = buyCarDto.DeliveryRequired
                };

                System.Diagnostics.Debug.WriteLine($"🧾 OrderDto: Client={orderDto.Client.PassportData.FirstName}, Product={orderDto.Product.Number}, DeliveryRequired={orderDto.DeliveryRequired}");

                // 4️⃣ Зберігаємо замовлення
                _orderService.Add(orderDto);
                System.Diagnostics.Debug.WriteLine("✅ Order created successfully.");

                // 5️⃣ Оновлюємо статус товару (вже не в наявності)
                product.InStock = false;
                _productRepository.Update(product);
                System.Diagnostics.Debug.WriteLine("🟢 Product marked as sold (InStock = false)");

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"❌ BuyCar ERROR: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"STACK TRACE: {ex.StackTrace}");
                MessageBox.Show($"Помилка при створенні замовлення: {ex.Message}");
                return false;
            }

        }

        private string GenerateProductNumber()
        {
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}
