using E_commerce.Models.Models;
using E_commerce.Repository.CategoryRepository;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace E_commerce_project.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryManagementController : Controller
    {
        private readonly ICategoryRepository _categoryrepository;
        public CategoryManagementController(ICategoryRepository categoryRepository) {
            _categoryrepository = categoryRepository;
        }
        [HttpPost]
        public async Task<Category> AddCategory(CategoryVM category)
        {
            var productcategory = await _categoryrepository.AddCategory(category);
            return productcategory;
        }
        [HttpPut("{id}")]
        public async Task<Category> UpdateCategory(int id, CategoryVM category)
        {
            if (id != category.Id)
                throw new ArgumentException("ID mismatch");

            var updatedcategory = await _categoryrepository.UpdateCategory(id, category);
            return updatedcategory;
        }
        [HttpGet("{id}")]
        public async Task<Category> GetCategoryById(int categoryid)
        {
            var getcategory = await _categoryrepository.GetCategoryById(categoryid);
            return getcategory;
        }
        [HttpGet]
        public async Task <List<Category>> GetAllCategories()
        {
            var allcategories = await _categoryrepository.GetAllCategory();
            return allcategories;
        }
    }
}
