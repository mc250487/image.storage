using Image.Storage.Configuration;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;

namespace Image.Storage.InternalServices.Impl
{
    public class ImagePreviewProvider : IImagePreviewProvider
    {
        private readonly IImageStorageConfiguration _configuration;

        public ImagePreviewProvider(IImageStorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Stream Create(Stream image)
        {
            var stream = new MemoryStream();

            using (var img = SixLabors.ImageSharp.Image.Load(image, out var format))
            {
                img.Mutate(x => x.Resize(new ResizeOptions
                {
                    Mode = ResizeMode.Max,
                    Size = new Size(_configuration.ImagePreviewWidth, _configuration.ImagePreviewHeight)
                }));

                img.Save(stream, format);
            }

            stream.Position = 0;

            return stream;
        }
    }
}
