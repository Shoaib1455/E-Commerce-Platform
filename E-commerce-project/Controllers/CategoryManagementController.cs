using E_commerce.Models.Models;
using E_commerce.Repository.CategoryRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryManagementController : Controller
    {
       private readonly ICategoryRepository _categoryrepository;
       public CategoryManagementController(ICategoryRepository categoryRepository) {
            _categoryrepository = categoryRepository;
        }
        [HttpPost("AddCategory")]
        public async Task<Category> AddCategory(CategoryVM category)
        {
            var productcategory = await _categoryrepository.AddCategory(category);
            return productcategory; 
        }
        [HttpPut("UpdateCategory")]
        public async Task<Category> UpdateCategory(CategoryVM category)
        {
            var updatedcategory = await _categoryrepository.UpdateCategory(category);
            return updatedcategory;
        }
        [HttpGet("CategoryById")]
        public async Task<Category> GetCategoryById(int categoryid)
        {
            var getcategory = await _categoryrepository.GetCategoryById(categoryid);
            return getcategory;
        }
        [HttpGet("AllCategories")]
        public async Task <List<Category>> GetAllCategories(int categoryid)
        {
            var allcategories = await _categoryrepository.GetAllCategory();
            return allcategories;
        }
    }
}
