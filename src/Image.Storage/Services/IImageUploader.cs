using System.Collections.Generic;
using System.Threading.Tasks;
using Image.Storage.Protocol;
using Microsoft.AspNetCore.Http;

namespace Image.Storage.Services
{
    public interface IImageUploader
    {
        Task<List<ImageInfo>> UploadAsync(UploadedImage[] images);

        List<ImageInfo> Upload(IFormFile[] images);
    }
}