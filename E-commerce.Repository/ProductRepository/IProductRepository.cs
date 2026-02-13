using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.ProductRepository
{
    public interface IProductRepository
    {
        public  Task<Product> AddProduct([FromForm] ProductVM product, int sellerid);
        public  Task<Product> UpdateProduct(ProductVM product);
        public  Task<bool> DeleteProduct(long id);
        public  Task<ProductVM> GetProductById(int productId);
        public Task<List<ProductVM>> GetAllProducts();
    }
}
