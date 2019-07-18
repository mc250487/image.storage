using JetBrains.Annotations;

namespace Image.Storage.Protocol
{
    public class ImageFile
    {
        [NotNull]
        public string FileId { get; set; }
    }

    public class LoadImageFileRequest
    {
        [NotNull]
        public string FileId { get; set; }
    }
}
