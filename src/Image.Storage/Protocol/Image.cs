using System.IO;

namespace Image.Storage.Protocol
{
    public class ImageInfo
    {
        public string Id { get; set; }

        public string MimeType { get; set; }
    }

    public class Image: ImageInfo
    {
        public Stream Content { get; set; }
    }

    public class SaveImageRequest
    {
        public string Name { get; set; }

        public Stream Content { get; set; }
    }

    public class ImageRequest
    {
        public string Id { get; set; }

        public bool Preview { get; set; }
    }
}
