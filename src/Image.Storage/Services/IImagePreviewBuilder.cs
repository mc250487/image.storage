using System.IO;
using System.Threading.Tasks;

namespace Image.Storage.Services
{
    public interface IImagePreviewBuilder
    {
        Task BuildPreview(Stream srcImageStream, Stream previewImageStream);
    }
}