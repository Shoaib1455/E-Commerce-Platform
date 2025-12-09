using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.CategoryRepository
{
    public class CategoryRepository:ICategoryRepository
    {
        
        private readonly EcommerceContext _context;

        public CategoryRepository(EcommerceContext context)
        {
            
            _context = context;
        }
        public async Task<Category> AddCategory(CategoryVM categorydetails)
        {
            Category category = new Category()
            {
                Name = categorydetails.Name,
                Description = categorydetails.Description,
                Isactive = categorydetails.Isactive,
            };
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
            return category;

        }
        public async Task<Category> UpdateCategory(int id,CategoryVM categorydetails)
        {
            var oldcategory = await _context.Categories.Where(c=>c.Id== id).FirstOrDefaultAsync();
            Category category = new Category()
            {
                Name = categorydetails.Name ?? oldcategory.Name,
                Description = categorydetails.Description ?? oldcategory.Description,
                Isactive = categorydetails.Isactive ?? oldcategory.Isactive,

            };
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<Category> GetCategoryById(long categoryId)
        {
            var category = await _context.Categories.Where(u => u.Id == categoryId).FirstOrDefaultAsync();
            return category;
        }
        public async Task<List<Category>> GetAllCategory()
        {
            List<Category> category = await _context.Categories.ToListAsync();
            return category;
        }
        public async Task<bool> DeleteCategory(long id)
        {
            var category = await _context.Categories.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (category == null)
            {
                return false;
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
