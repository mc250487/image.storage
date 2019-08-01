using Image.Storage.Protocol;
using Image.Storage.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Image.Storage.Controllers
{
    [Route("images")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        [HttpPost]
        [Route("")]
        [Consumes("application/json")]
        public async Task<List<ImageInfo>> UploadAsync(UploadedImage[] images)
        {
            var uploader = HttpContext.RequestServices.GetService<IImageUploader>();

            return await uploader.UploadAsync(images).ConfigureAwait(continueOnCapturedContext: false);
        }

        [HttpPost]
        [Route("")]
        [Consumes(
            "image/gif",
            "image/jpeg",
            "image/pjpeg",
            "image/png",
            "image/svg+xml",
            "image/tiff",
            "image/vnd.microsoft.icon",
            "image/vnd.wap.wbmp",
            "image/webp"
        )]
        public async Task<ImageInfo> Upload()
        {
            UploadedImage image;

            using (var stream = new MemoryStream())
            {
                HttpContext.Request.Body.CopyTo(stream);

                image = new UploadedImage
                {
                    MimeType = Request.ContentType,
                    Content = stream.ToArray(),
                    Name = Guid.NewGuid().ToString("N")
                };
            }
            var uploader = HttpContext.RequestServices.GetService<IImageUploader>();

            return (await uploader.UploadAsync(new[] { image }).ConfigureAwait(continueOnCapturedContext: false)).First();
        }

        [HttpPost]
        [Route("")]
        [Consumes("multipart/form-data")]
        public List<ImageInfo> Upload(IFormFile[] images)
        {
            var uploader = HttpContext.RequestServices.GetService<IImageUploader>();

            return uploader.Upload(images);
        }

        [HttpGet]
        [Route("{id}")]
        public ActionResult Download(string id)
        {
            var imageRepository = HttpContext.RequestServices.GetService<IImageRepository>();

            var image = imageRepository.Load(new ImageRequest
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
            var imageRepository = HttpContext.RequestServices.GetService<IImageRepository>();

            var image = imageRepository.Load(new ImageRequest
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
            var imageRepository = HttpContext.RequestServices.GetService<IImageRepository>();

            var image = imageRepository.LoadInfo(new ImageRequest
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