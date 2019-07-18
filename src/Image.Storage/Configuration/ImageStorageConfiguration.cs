using Microsoft.Extensions.Configuration;

namespace Image.Storage.Configuration
{
    public interface IImageStorageConfiguration
    {
        string ImageFilesLocation { get; }

        string PreviewImageFilesLocation { get; }

        int ImagePreviewWidth { get; }


        int ImagePreviewHeight { get; }
    }

    public class ImageStorageConfiguration : IImageStorageConfiguration
    {
        public string ImageFilesLocation => _configuration.GetValue<string>("ImageFileStorage:Location");

        public string PreviewImageFilesLocation => _configuration.GetValue<string>("ImageFileStorage:PreviewLocation");

        public int ImagePreviewWidth => _configuration.GetValue<int>("ImagePreview:Width");

        public int ImagePreviewHeight => _configuration.GetValue<int>("ImagePreview:Height");

        private readonly IConfiguration _configuration;

        public ImageStorageConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
