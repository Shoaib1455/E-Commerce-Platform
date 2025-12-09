using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Repository.ProductRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers
{

    [Route("api/products")]
    [ApiController]
    public class ProductManagementController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductManagementController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpPost]
        public async Task<Product> AddProduct(ProductVM product)
        {

            var addedproduct = await _productRepository.AddProduct(product);
            return addedproduct;
        }
        [HttpPut("{id}")]
        public async Task<Product> UpdateProduct(ProductVM product, int id)
        {
            return await _productRepository.UpdateProduct(product);
        }

        [HttpGet("{id}")]
        public async Task<ProductVM> GetProductById(int id)
        {
            var getproduct = await _productRepository.GetProductById(id);
            return getproduct;
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<List<ProductVM>> GetAllProducts()
        {
            var productslist = await _productRepository.GetAllProducts();
            return productslist;
        }
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProduct(int pid)
        {
            var isproductdeleted=await _productRepository.DeleteProduct(pid);
            return isproductdeleted;
        }

    }
}
