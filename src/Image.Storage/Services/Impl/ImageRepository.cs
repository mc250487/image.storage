using Image.Storage.InternalServices;
using Image.Storage.Protocol;
using System;

namespace Image.Storage.Services.Impl
{
    public class ImageRepository : IImageRepository
    {
        private readonly IFileStorage _fileStorage;
        private readonly IImagePreviewProvider _imagePreviewProvider;

        public ImageRepository(IFileStorage fileStorage,
            IImagePreviewProvider imagePreviewProvider)
        {
            _fileStorage = fileStorage;
            _imagePreviewProvider = imagePreviewProvider;
        }

        public Protocol.ImageInfo Save(SaveImageRequest request)
        {
            var id = Guid.NewGuid().ToString("N");

            var imageFile = _fileStorage.Save(new SaveFileRequest
            {
                Id = id,
                Name = request.Name,
                Content = request.Content
            });

            var previewId = $"previews/{id}";
            var name = $"preview.{request.Name}";

            request.Content.Position = 0;

            using (var previewContent = _imagePreviewProvider.Create(request.Content))
            {
                _fileStorage.Save(new SaveFileRequest
                {
                    Id = previewId,
                    Name = name,
                    Content = previewContent
                });
            }

            return new Protocol.ImageInfo
            {
                Id = id,
                MimeType = imageFile.MimeType
            };
        }

        public Protocol.Image Load(ImageRequest request)
        {
            var id = request.Id;

            if (request.Preview)
            {
                id = $"previews/{id}";
            }

            var loadFileRequest = new LoadFileRequest
            {
                Id = id
            };

            var file = _fileStorage.Load(loadFileRequest);

            if (file != null)
            {
                var fileContent = _fileStorage.LoadContent(loadFileRequest);

                if (fileContent != null)
                {
                    return new Protocol.Image
                    {
                        Id = request.Id,
                        MimeType = file.MimeType,
                        Content = fileContent
                    };
                }
            }

            return null;
        }

        public ImageInfo LoadInfo(ImageRequest request)
        {
            var file = _fileStorage.Load(new LoadFileRequest
            {
                Id = request.Id
            });

            if (file != null)
            {
                return new ImageInfo
                {
                    Id = request.Id,
                    MimeType = file.MimeType
                };
            }

            return null;
        }
    }
}
