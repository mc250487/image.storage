using Image.Storage.Protocol;
using JetBrains.Annotations;
using System.IO;
using System.Threading.Tasks;

namespace Image.Storage.Services
{
    public interface IImageFileStorage
    {
        [NotNull]
        [ItemNotNull]
        Task<ImageFile> SaveImageAsync([NotNull] Stream stream);

        [CanBeNull]
        Stream LoadImage([NotNull] LoadImageFileRequest request);

        [CanBeNull]
        Stream LoadPreview([NotNull] LoadImageFileRequest request);
    }

    public static class ImageFileStorageExtenstions
    {
        public static async Task<ImageFile> SaveImageAsync(this IImageFileStorage fileStorage, [NotNull] byte[] content)
        {
            using (var stream = new MemoryStream(content))
            {
                return await fileStorage.SaveImageAsync(stream);
            }
        }
    }
}