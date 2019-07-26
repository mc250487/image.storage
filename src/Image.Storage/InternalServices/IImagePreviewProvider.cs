using JetBrains.Annotations;
using System;
using System.IO;

namespace Image.Storage.InternalServices
{
    public interface IImagePreviewProvider
    {
       // [NotNull]
       // ImagePreview Create([NotNull]Stream image);

        Stream Create([NotNull]Stream image);
    }

    public class ImagePreview : IDisposable
    {
        private readonly string _tmpFilePath;
        private readonly Stream _stream;
        private bool _disposed;
        private object _locker = new object();

        public Stream Content => _stream;

        public ImagePreview()
        {
            _tmpFilePath = Path.GetTempFileName();

            _stream = new FileStream(_tmpFilePath, FileMode.Create);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_locker)
                {
                    if (!_disposed)
                    {
                        _stream.Close();
                        File.Delete(_tmpFilePath);
                        _disposed = true;
                    }
                }
            }
        }

        ~ImagePreview()
        {
            Dispose();
        }
    }
}