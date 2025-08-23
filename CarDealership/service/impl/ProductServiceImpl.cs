using CarDealership.entity;
using CarDealership.mapper;
using CarDealership.dto;
using CarDealership.enums;
using CarDealership.repo;

namespace CarDealership.service.impl
{
    public class ProductServiceImpl : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductServiceImpl(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<ProductDto> GetAll()
        {
            return _productRepository.GetAll().Select(ProductMapper.ToDto).ToList();
        }

        public ProductDto? GetById(int id)
        {
            return ProductMapper.ToDto(_productRepository.GetById(id));
        }

        public bool Create(Product product)
        {
            if (_productRepository.ExistsByNumber(product.Number))
                return false;

            _productRepository.Add(product);
            return true;
        }

        public bool Update(Product product)
        {
            if (!_productRepository.ExistsByNumber(product.Number))
                return false;

            _productRepository.Update(product);
            return true;
        }

        public bool Delete(int id)
        {
            var product = _productRepository.GetById(id);
            if (product == null)
                return false;

            _productRepository.Delete(id);
            return true;
        }

        public bool ExistsByNumber(string number)
        {
            return _productRepository.ExistsByNumber(number);
        }

        // Test method to verify database connection
        public bool TestDatabaseConnection()
        {
            try
            {
                var products = _productRepository.GetAll();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Database connection test failed: {ex.Message}");
                return false;
            }
        }



        private string GenerateProductNumber()
        {
            // Generate a unique product number with timestamp
            return $"PROD-{DateTime.Now:yyyyMMdd}-{DateTime.Now:HHmmss}-{Guid.NewGuid().ToString().Substring(0, 8)}";
        }
    }
}