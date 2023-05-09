using RedisExampleApp.API.Models;
using RedisExampleApp.API.Repositories;

namespace RedisExampleApp.API.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Product> CreateAsync(Product product)
        {
           return await _productRepository.CreateAsync(product);
        }

        public async Task<List<Product>> GetAsync()
        {
            return await _productRepository.GetAsync();
        }

        public Task<Product> GetByIdAsync(int id)
        {
            return _productRepository.GetByIdAsync(id);
        }
    }
}
