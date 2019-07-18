using Image.Storage.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;
using System.Threading.Tasks;

namespace Image.Storage.Services.Impl
{
    public class ImagePreviewBuilder: IImagePreviewBuilder
    {
        private readonly IImageStorageConfiguration _configuration;

        public ImagePreviewBuilder(IImageStorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task BuildPreview(Stream srcImageStream, Stream previewImageStream)
        {
            IImageFormat format;
            var img = SixLabors.ImageSharp.Image.Load(srcImageStream, out format);
            img.Mutate(x=>x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(_configuration.ImagePreviewWidth, _configuration.ImagePreviewHeight)
            }));
            
            img.Save(previewImageStream, format);

            return Task.CompletedTask;
        }
    }
}
