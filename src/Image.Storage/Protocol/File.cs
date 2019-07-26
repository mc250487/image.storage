using System.IO;
using JetBrains.Annotations;

namespace Image.Storage.Protocol
{
    public class File
    {
        public string Id { get; set; }

        public string MimeType { get; set; }

        public string Name { get; set; }
    }

    public class UploadImageRequest
    {
        public LocalImage[] LocalImages { get; set; }

        public RemoteImage[] RemoteImages { get; set; }
    }

    public class LocalImage
    {
        public string MimeType { get; set; }

        public string Name { get; set; }

        public byte[] Content { get; set; }
    }

    public class RemoteImage
    {
        public string Url { get; set; }
    }

    public class SaveFileRequest
    {
        [CanBeNull]
        public string Id { get; set; }

        [NotNull]
        public string Name { get; set; }

        [NotNull]
        public Stream Content { get; set; }
    }

    public class LoadFileRequest
    {
        [NotNull]
        public string Id { get; set; }
    }
}
