using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Image.Storage.Configuration;
using Image.Storage.Protocol;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Image.Storage.Services.Impl
{
    public class ImageUploader : IImageUploader
    {
        private readonly IImageRepository _imageRepository;
        private readonly IImageStorageConfiguration _configuration;

        public ImageUploader(
            IImageRepository imageRepository,
            IImageStorageConfiguration configuration
            )
        {
            _imageRepository = imageRepository;
            _configuration = configuration;
        }

        public async Task<List<ImageInfo>> UploadAsync(UploadedImage[] images)
        {
            var result = new List<ImageInfo>();
            HttpClient httpClient = null;
            byte[] content;
            string name;

            foreach (var image in images)
            {
                if (!string.IsNullOrWhiteSpace(image.Url))
                {
                    httpClient = httpClient ?? new HttpClient();

                    name = !string.IsNullOrWhiteSpace(image.Name) ? image.Name : image.Url.Split('/').Last();
                    var response = await httpClient.GetAsync(image.Url);

                    CheckImageMimeType(response.Content.Headers.ContentType.MediaType);
                    content = await response.Content.ReadAsByteArrayAsync();

                }
                else
                {
                    CheckImageMimeType(image.MimeType);
                    name = image.Name;
                    content = image.Content;
                }

                using (var stream = new MemoryStream(content))
                {
                    var savedImage = _imageRepository.Save(new SaveImageRequest
                    {
                        Name = name,
                        Content = stream
                    });

                    result.Add(savedImage);
                }
            }

            return result;
        }

        public List<ImageInfo> Upload(IFormFile[] images)
        {
            var result = new List<ImageInfo>();

            foreach (var image in images)
            {
                CheckImageMimeType(image.ContentType);

                using (var stream = image.OpenReadStream())
                {
                    var savedImage = _imageRepository.Save(new SaveImageRequest
                    {
                        Name = image.FileName,
                        Content = stream
                    });

                    result.Add(savedImage);
                }
            }

            return result;
        }

        private void CheckImageMimeType(string mimeType)
        {
            if (!_configuration.ImageMimeTypes.Contains(mimeType.ToLowerInvariant()))
            {
                var mimeTypes = string.Join(',', _configuration.ImageMimeTypes.AsEnumerable());
                throw new ImageStorageException($"Mime types '{mimeTypes}' are expected. But '{mimeType}' are received.");
            }
        }
    }
}
