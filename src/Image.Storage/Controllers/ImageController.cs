using Image.Storage.Protocol;
using Image.Storage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Threading.Tasks;
using Image.Storage.InternalServices;

namespace Image.Storage.Controllers
{
    [Route("images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImageController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("")]
        public async Task<Protocol.ImageInfo[]> UploadAsync(
            [ModelBinder(BinderType = typeof(JsonModelBinder))]
            UploadImageRequest json,
            List<IFormFile> images
            )
        {
            using (var stream = images[0].OpenReadStream())
            {
                var image = _imageRepository.Save(new SaveImageRequest
                {
                    Name = images[0].FileName,
                    Content = stream
                });

                await Task.CompletedTask;

                return new[] { image };
            }
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Download(string id)
        {
            var image = _imageRepository.Load(new ImageRequest
            {
                Id = id
            });

            if (image != null)
            {
                return File(image.Content, image.MimeType); 
            }
            return NotFound();

        }

        [HttpGet]
        [Route("{id}/preview")]
        public ActionResult DownloadPreview(string id)
        {
            var image = _imageRepository.Load(new ImageRequest
            {
                Id = id,
                Preview = true
            });

            if (image != null)
            {
                return File(image.Content, image.MimeType);
            }

            return NotFound();
        }

        [HttpGet]
        [Route("{id}/info")]
        public ActionResult GetInfo(string id)
        {
            var image = _imageRepository.LoadInfo(new ImageRequest
            {
                Id = id
            });

            if (image != null)
            {
                return Ok(image);
            }

            return NotFound();
        }
    }
}