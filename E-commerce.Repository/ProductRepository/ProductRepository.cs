using E_commerce.Models.Data;
using E_commerce.Models.Models;
using E_commerce.Repository.InventoryRepository;
using E_commerce.Services.Caching;
using E_commerce.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace E_commerce.Repository.ProductRepository
{
    public class ProductRepository : IProductRepository
    {
        private readonly EcommerceContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICacheService _cache;
        private readonly IInventoryRepository _inventoryRepository;
        private const string ProductsCacheKey = "products_all";

        public ProductRepository(EcommerceContext context , IHttpContextAccessor httpContextAccessor, ICacheService cache, IInventoryRepository inventoryRepository)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _inventoryRepository = inventoryRepository;
            _cache = cache;
        }
        //public async Task<Product> AddProductWithInventoryAsync(ProductVM dto, int sellerId)
        //{
        //    using var transaction = await _context.Database.BeginTransactionAsync();

        //    // Add product
        //    var product = await AddProduct(dto);

        //    // Add initial inventory
        //    await _inventoryRepository.AddInitialInventoryAsync(product.Id, dto.quantity, sellerId);

        //    await transaction.CommitAsync();
        //    return product;
        //}
        public async Task<Product> AddProduct([FromForm] ProductVM productdetails,int sellerid)
        {

            //var imageUrl=UploadImage(productdetails.ImageUrl);
            var imageUrl= UploadImagesInFile(productdetails.ThumbnailImage);
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

            foreach (var image in productdetails.ProductImages)
            {
                string url = UploadImagesInFile(image);
                _context.Productimages.Add(new Productimage
                {
                    Productid = product.Id, // example
                    Imageurl = url,
                    Createdat=DateTime.Now
                });
            }
            await _context.SaveChangesAsync();
            await _inventoryRepository.AddInitialInventoryAsync(product.Id, productdetails.Quantity,sellerid);
            //_cache.Remove(CacheKeys.AllProducts);
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
            var product = await _context.Products.AsNoTracking().Include(i=>i.Productimages).Where(u => u.Id == productId).FirstOrDefaultAsync();
            var fullImageUrls = product.Productimages
    .Select(path => GetImageFullPath(path.Imageurl))
    .ToList();
            ProductVM products = new ProductVM
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                ImageUrl = string.IsNullOrEmpty(product.Imageurl) ? null : GetImageFullPath(product.Imageurl),
                MultipleImagesUrl = fullImageUrls ==null ? null : fullImageUrls,
            };
            return products;
        }
        public async Task<List<ProductVM>> GetAllProducts()
        {
            var cachedProducts = await _cache.GetAsync<List<ProductVM>>(ProductsCacheKey);
            if (cachedProducts != null)
            {
                return cachedProducts;
            }
            var products = await _context.Products.AsNoTracking().ToListAsync();
            var  productlist = products.
                Select(p=>new ProductVM{
                    Id=p.Id,
                    Name=p.Name,
                    Description=p.Description,
                    Price=p.Price,
                    ImageUrl= string.IsNullOrEmpty(p.Imageurl)? null: GetImageFullPath(p.Imageurl)
                }).ToList();
            await _cache.SetAsync(ProductsCacheKey, productlist, TimeSpan.FromMinutes(10));
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
        private string UploadImagesInFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".webp",".jfif" };
            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Invalid image type");

            var fileName = $"{Guid.NewGuid()}{extension}";

            var mediaFolder = Path.Combine(Directory.GetCurrentDirectory(), "Media");

            if (!Directory.Exists(mediaFolder))
                Directory.CreateDirectory(mediaFolder);

            var savePath = Path.Combine(mediaFolder, fileName);

            using var stream = new FileStream(savePath, FileMode.Create);
            file.CopyTo(stream);

            // relative path for DB
            return Path.Combine("Media", fileName).Replace("\\", "/");
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
