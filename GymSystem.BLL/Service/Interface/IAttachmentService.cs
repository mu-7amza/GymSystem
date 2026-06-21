using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GymSystem.BLL.Service.Interface
{
    public interface IAttachmentService
    {
        Task<string?> UploadAsync(IFormFile formFile, string fileName , string folderName , CancellationToken ct);
        bool Delete(string fileName, string folderName);
        string? GetPhoto(string fileName,string folderName, CancellationToken ct);

    }
}
