using System;

namespace Image.Storage
{
    public class ImageStorageException: Exception
    {
        public ImageStorageException(string message) : base(message)
        {
        }
    }
}
