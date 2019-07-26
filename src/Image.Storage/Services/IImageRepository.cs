using Image.Storage.Protocol;
using JetBrains.Annotations;

namespace Image.Storage.Services
{
    public interface IImageRepository
    {
        [NotNull]
        Protocol.ImageInfo Save([NotNull]SaveImageRequest request);

        [CanBeNull]
        Protocol.Image Load([NotNull]ImageRequest request);

        [CanBeNull]
        Protocol.ImageInfo LoadInfo([NotNull]ImageRequest request);
    }
}