namespace Image.Storage.Protocol
{
    public class Image
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
}
