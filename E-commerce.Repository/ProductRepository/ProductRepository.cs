using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_commerce.Repository.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        
        private readonly EcommerceContext _context;
        public ProductRepository(EcommerceContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProduct(ProductVM productdetails)
        {
            Product product = new Product()
            {
                Name = productdetails.Name,
                Description = productdetails.Description,
                Price = productdetails.Price,
                Sku = productdetails.Sku,
                Isactive = productdetails.Isactive,


            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;

        }//problem in update
        public async Task<Product> UpdateProduct(ProductVM productdetails)
        {
            var oldproduct = await _context.Products.AsNoTracking().Where(p=>p.Id== productdetails.Id).FirstOrDefaultAsync();
            if (oldproduct == null) return null;
            Product product = new Product()
            {
                Id= oldproduct.Id,
                Name = productdetails.Name ?? oldproduct.Name,
                Description = productdetails.Description ?? oldproduct.Description,
                Price = (productdetails.Price != 0) ? productdetails.Price : oldproduct.Price,
                Sku = productdetails.Sku ?? oldproduct.Sku,
                Isactive = productdetails.Isactive ?? oldproduct.Isactive,

            };
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
        public async Task<Product> GetProductById(long productId)
        {
            var product = await _context.Products.Where(u => u.Id == productId).FirstOrDefaultAsync();
            return product;
        }
        public async Task<List<Product>> GetAllProducts()
        {
            List<Product> product = await _context.Products.ToListAsync();
            return product;
        }
        public async Task<bool> DeleteProduct(long id) {
            var product = await _context.Products.Where(p=>p.Id==id).FirstOrDefaultAsync();
            if (product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
