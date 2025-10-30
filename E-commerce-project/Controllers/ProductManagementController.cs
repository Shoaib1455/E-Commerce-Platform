using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Repository.ProductRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductManagementController : Controller
    {
        private readonly IProductRepository _productRepository;
        public ProductManagementController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        [HttpPost("addproduct")]
        public async Task<Product> AddProduct(ProductVM product)
        {
            var addedproduct = await _productRepository.AddProduct(product);
            return addedproduct;
        }
        [HttpPut("updateproduct")]
        public async Task<Product> UpdateProduct(ProductVM product)
        {
            return await _productRepository.UpdateProduct(product);
        }

        [HttpGet("getproductbyid")]
        public async Task<Product> GetProductById(long pid)
        {
            var getproduct = await _productRepository.GetProductById(pid);
            return getproduct;
        }
        [HttpGet]
        public async Task<List<Product>> GetAllProducts()
        {
            var productslist = await _productRepository.GetAllProducts();
            return productslist;
        }
        [HttpDelete("deleteproduct")]
        public async Task<bool> DeleteProduct(int pid)
        {
            var isproductdeleted=await _productRepository.DeleteProduct(pid);
            return isproductdeleted;
        }

    }
}
