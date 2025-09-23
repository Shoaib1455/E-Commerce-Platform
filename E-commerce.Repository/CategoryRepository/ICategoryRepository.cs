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
        public Task<Product> AddProduct(ProductVM product);
        public Task<Product> UpdateProduct(ProductVM product);
        public Task<bool> DeleteProduct(long id);
        public Task<Product> GetProductById(long productId);
    }
}
