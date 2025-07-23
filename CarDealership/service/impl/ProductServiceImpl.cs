using CarDealership.entity;
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

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product? GetById(int id)
        {
            return _productRepository.GetById(id);
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
    }
}