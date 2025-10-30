using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace E_commerce.Services
{
    public  class FileStorageService
    {
        public FileStorageService() 
        { 
        
        }
        public async Task<string> UploadFileAsync(IFormFile file, string folderName)
        {
            // Define where to save the file (Media folder in project root)
            string mediaPath = Path.Combine(Directory.GetCurrentDirectory(), "Media", folderName);
            Directory.CreateDirectory(mediaPath); // Ensure folder exists

            // Generate unique filename to avoid conflicts
            string uniqueName = $"{Guid.NewGuid()}_{file.FileName}";
            string filePath = Path.Combine(mediaPath, uniqueName);

            // Save file to disk
            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            // Return relative path (used for frontend to access)
            string relativePath = Path.Combine("Media", folderName, uniqueName).Replace("\\", "/");
            return relativePath;
        }

    }
}
