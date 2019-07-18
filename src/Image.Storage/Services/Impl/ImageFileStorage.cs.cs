using Image.Storage.Configuration;
using Image.Storage.Protocol;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Image.Storage.Services.Impl
{
    public class ImageFileStorage : IImageFileStorage
    {
        private readonly IImageStorageConfiguration _configuration;
        private readonly IImagePreviewBuilder _imagePreviewBuilder;

        public ImageFileStorage(IImageStorageConfiguration configuration, IImagePreviewBuilder imagePreviewBuilder)
        {
            _configuration = configuration;
            _imagePreviewBuilder = imagePreviewBuilder;
        }

        public async Task<ImageFile> SaveImageAsync(Stream content)
        {
            var fileId = Guid.NewGuid().ToString("N");
            var filePath = Path.Combine(_configuration.ImageFilesLocation, fileId);
            var previewFilePath = Path.Combine(_configuration.PreviewImageFilesLocation, fileId);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await content.CopyToAsync(fileStream);
            }

            content.Position = 0;


            using (var fileStream = new FileStream(previewFilePath, FileMode.Create))
            {
                await _imagePreviewBuilder.BuildPreview(content, fileStream);
            }

            return new ImageFile
            {
                FileId = fileId
            };
        }

        public Stream LoadImage(LoadImageFileRequest request)
        {
            var filePath = Path.Combine(_configuration.ImageFilesLocation, request.FileId);

            return !File.Exists(filePath) ? null : new FileStream(filePath, FileMode.Open);
        }

        public Stream LoadPreview(LoadImageFileRequest request)
        {
            var filePath = Path.Combine(_configuration.PreviewImageFilesLocation, request.FileId);

            return !File.Exists(filePath) ? null : new FileStream(filePath, FileMode.Open);
        }
    }
}
