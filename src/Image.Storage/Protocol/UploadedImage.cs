using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace Image.Storage.Protocol
{
    public class UploadedImage
    {
        [CanBeNull]
        public string MimeType { get; set; }

        [CanBeNull]
        public string Name { get; set; }

        [CanBeNull]
        public byte[] Content { get; set; }

        [CanBeNull]
        public string Url { get; set; }
    }
}
