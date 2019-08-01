using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Image.Storage.Configuration
{
    public interface IImageStorageConfiguration
    {
        string StorageLocation { get; }

        string StorageDbFileName { get; }

        int ImagePreviewWidth { get; }

        int ImagePreviewHeight { get; }

        HashSet<string> ImageMimeTypes { get; }
    }

    public class ImageStorageConfiguration : IImageStorageConfiguration
    {
        public string StorageLocation =>
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData,
                    Environment.SpecialFolderOption.Create), "Image.Storage");

        public string StorageDbFileName => _configuration.GetValue<string>("Storage:DbFileName");

        public int ImagePreviewWidth => _configuration.GetValue<int>("ImagePreview:Width");

        public int ImagePreviewHeight => _configuration.GetValue<int>("ImagePreview:Height");

        public HashSet<string> ImageMimeTypes { get; } = new HashSet<string>
        {
            "image/gif",
            "image/jpeg",
            "image/pjpeg",
            "image/png",
            "image/svg+xml",
            "image/tiff",
            "image/vnd.microsoft.icon",
            "image/vnd.wap.wbmp",
            "image/webp"
        };

        private readonly IConfiguration _configuration;

        public ImageStorageConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
    }
}
