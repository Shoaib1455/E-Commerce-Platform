using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Http;
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
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductRepository(EcommerceContext context , IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Product> AddProduct(ProductVM productdetails)
        {
            var imageUrl=UploadImage(productdetails.ImageUrl);
            var categoryid = await _context.Categories.Where(c => c.Name == productdetails.CategoryName).Select(c => c.Id).FirstOrDefaultAsync();

            Product product = new Product()
            {
                Name = productdetails.Name,
                Description = productdetails.Description,
                Price = productdetails.Price,
                Sku = productdetails.Sku,
                Isactive = productdetails.Isactive,
                Categoryid= categoryid,
                Imageurl = imageUrl,
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
        public async Task<ProductVM> GetProductById(int productId)
        {
            var product = await _context.Products.Where(u => u.Id == productId).FirstOrDefaultAsync();
            ProductVM products = new ProductVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = product.Imageurl
            };
            return products;
        }
        public async Task<List<ProductVM>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            var  productlist = products.
                Select(p=>new ProductVM{
                    Id=p.Id,
                    Name=p.Name,
                    Description=p.Description,
                    Price=p.Price,
                    ImageUrl= string.IsNullOrEmpty(p.Imageurl)? null: GetImageFullPath(p.Imageurl)
                }).ToList();
            
            return productlist;
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

        private string UploadImage(string imageUrl)
        {
            var base64Data = imageUrl;

            if (base64Data.Contains(","))
            {
                base64Data = base64Data.Split(',')[1]; // remove metadata (e.g. "data:image/png;base64,")
            }
            byte[] fileBytes = Convert.FromBase64String(base64Data);
            string extension =GetFileExtensionFromBase64(imageUrl);
            string fileName = $"{Guid.NewGuid()}{extension}";
            string mediaFolder = Path.Combine(Directory.GetCurrentDirectory(),"Media");
            if (!Directory.Exists(mediaFolder))
                Directory.CreateDirectory(mediaFolder);

            // Full path where file will be saved
            string savePath = Path.Combine(mediaFolder, fileName);

            // Write the file to disk
            File.WriteAllBytes(savePath, fileBytes);

            // Return relative path to store in DB
            return Path.Combine("Media", fileName);
            

        }
        private string GetFileExtensionFromBase64(string base64String)
        {
            if (base64String.StartsWith("data:image/png")) return ".png";
            if (base64String.StartsWith("data:image/jpeg")) return ".jpg";
            if (base64String.StartsWith("data:application/pdf")) return ".pdf";
            return ".bin"; // fallback
        }
        private  string GetImageFullPath(string imagePath)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}/";
            return $"{baseUrl}{imagePath.Replace("\\", "/")}";
        }
    }

}
