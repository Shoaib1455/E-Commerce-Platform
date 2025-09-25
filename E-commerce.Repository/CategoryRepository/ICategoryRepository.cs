using E_commerce.Models.Models;
using E_commerce.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.CategoryRepository
{
    public interface ICategoryRepository
    {
        public Task<Category> AddCategory(CategoryVM category);
        public Task<Category> UpdateCategory(CategoryVM category);
        public Task<bool> DeleteCategory(long id);
        public Task<Category> GetCategoryById(long categoryId);
        public Task<List<Category>> GetAllCategory();
    }
}
