using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymSystem.BLL.Service.Interface;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace GymSystem.BLL.Service.Class
{
    public class AttachmentService : IAttachmentService
    {
        private readonly long maxFileSize = 5 * 1024 * 1024; // 5 MB
        private readonly string[] AllowsExtensions = { ".jpg", ".jpeg", ".png" };
        private readonly ILogger<AttachmentService> logger;
        private readonly IWebHostEnvironment _environment;

        public AttachmentService(ILogger<AttachmentService> logger, IWebHostEnvironment environment)
        {
            this.logger = logger;
            _environment = environment;
        }

        public bool Delete(string fileName, string folderName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
            {
                return false;
            }
            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, folderName, fileName);
                if (!Path.Exists(fullPath)) return false;
                File.Delete(fullPath);
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Failed to delete the file : {fileName}");
                return false;
            }
        }

        public string? GetPhoto(string fileName, string folderName, CancellationToken ct)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
                    return null;

                var photoPath = $"{folderName}/{fileName}";
                return photoPath;

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "file not exist");
                return null;
            }
        }
        public async Task<string?> UploadAsync(IFormFile formFile, string fileName, string folderName, CancellationToken ct)
        {
            
                if (formFile is null ) return null;
                if (formFile.Length == 0) return null;
                if(formFile.Length > maxFileSize)
                {
                    logger.LogWarning("Reject File too large");
                    return null;
                }

                var extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension) || !AllowsExtensions.Contains(extension))
                {
                    logger.LogWarning("Invalid file extension");
                    return null;
                }

                var wwwPath = _environment.WebRootPath;
                var path = Path.Combine(wwwPath, folderName);
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                

                var storedFileName = $"{Guid.NewGuid()}{extension}";

                // Full Path
                var filepath = Path.Combine(path, storedFileName);

                try
                {
                    await using var fs = new FileStream(filepath, FileMode.Create,FileAccess.Write,FileShare.None);
                    await formFile.CopyToAsync(fs,ct);
                    return storedFileName;
                }
                catch(Exception ex)
                {
                    logger.LogError(ex, $"Failed to upload the file : {storedFileName}");
                     return null;
                }
        }
    }
}

