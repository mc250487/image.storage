using Image.Storage.Protocol;
using Image.Storage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Image.Storage.Controllers
{
    [Route("images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageFileStorage _imageFileStorage;

        public ImageController(IImageFileStorage imageFileStorage)
        {
            _imageFileStorage = imageFileStorage;
        }

        [HttpPost]
        [Route("")]
        public async Task<Protocol.Image[]> UploadAsync(
            [ModelBinder(BinderType = typeof(JsonModelBinder))]
            UploadImageRequest json,
            List<IFormFile> images
            )
        {
            var files = new List<Protocol.Image>();

            foreach (var image in images)
            {
                if (image.Length > 0)
                {
                    using (var stream = image.OpenReadStream())
                    {
                        var imageFile = await _imageFileStorage.SaveImageAsync(stream);

                        files.Add(new Protocol.Image
                        {
                            Id = imageFile.FileId,
                            MimeType = image.ContentType,
                            Name = image.FileName
                        });
                    }
                }
            }

            if ((json?.LocalImages?.Length ?? 0) > 0)
            {
                foreach (var image in json.LocalImages)
                {
                    var fileInfo = await _imageFileStorage.SaveImageAsync(image.Content);

                    files.Add(new Protocol.Image
                    {
                        Id = fileInfo.FileId,
                        MimeType = image.MimeType,
                        Name = image.Name
                    });
                }
            }

            return files.ToArray();
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Download(string id)
        {
            var stream = _imageFileStorage.LoadImage(new LoadImageFileRequest
            {
                FileId = id
            });

            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "image/jpeg");
        }

        [HttpGet]
        [Route("{id}/preview")]
        public ActionResult DownloadPreview(string id)
        {
            var stream = _imageFileStorage.LoadPreview(new LoadImageFileRequest
            {
                FileId = id
            });

            if (stream == null)
            {
                return NotFound();
            }

            return File(stream, "image/jpeg");
        }
    }
}