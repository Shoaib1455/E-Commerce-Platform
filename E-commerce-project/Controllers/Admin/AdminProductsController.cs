using E_commerce.Models.Models;
using E_commerce.Repository.ProductRepository;
using E_commerce.Repository.UserRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers.Admin
{
    public class AdminProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        public AdminProductsController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<List<ProductVM>> GetAllProducts()
        {
            var products = await _productRepository.GetAllProducts();
            return products;
        }
        public async Task<Product> GetProductById(long productid)
        {
            var product = await _productRepository.GetProductById(productid);
            return product;
        }
        public async Task<Product> AddProduct(ProductVM product)
        {
            var products = await _productRepository.AddProduct(product);
            return products;
        }
        public async Task<Product> UpdateProduct(ProductVM product)
        {
            var products = await _productRepository.UpdateProduct(product);
            return products;
        }
    }
}
