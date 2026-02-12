using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Repository.ProductRepository;
using E_commerce.ViewModels;
using E_commerce.ViewModels.Common;
using E_commerce_project.Common;
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
        public async Task<Product> AddProduct([FromForm] ProductVM product)
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
        public async Task<ApiResponse<List<ProductVM>>> GetAllProducts()
        {
            var productslist = await _productRepository.GetAllProducts();
            if (productslist == null)
            {
                return new ApiResponse<List<ProductVM>>
                {
                    IsSuccess = false,
                    Message = ApiMessages.NoDataFound,
                    
                };
            }
            return new ApiResponse<List<ProductVM>>
            {
                IsSuccess = true,
                Message = ApiMessages.Success,
                Result= productslist
            };
        }
        [HttpDelete("{id}")]
        public async Task<bool> DeleteProduct(int pid)
        {
            var isproductdeleted=await _productRepository.DeleteProduct(pid);
            return isproductdeleted;
        }

    }
}
