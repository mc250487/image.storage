using System.IO;
using Image.Storage.Protocol;
using JetBrains.Annotations;
using File = Image.Storage.Protocol.File;

namespace Image.Storage.Services
{
    public interface IFileStorage
    {
        [NotNull]
        File Save([NotNull]SaveFileRequest request);

        File Load(LoadFileRequest request);

        Stream LoadContent(LoadFileRequest request);
    }
}