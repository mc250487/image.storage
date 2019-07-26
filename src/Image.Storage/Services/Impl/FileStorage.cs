using Image.Storage.Configuration;
using Image.Storage.Protocol;
using LiteDB;
using System;
using System.IO;
using File = Image.Storage.Protocol.File;

namespace Image.Storage.Services.Impl
{
    public class FileStorage : IFileStorage
    {
        private readonly IImageStorageConfiguration _configuration;

        public FileStorage(IImageStorageConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Protocol.File Save(SaveFileRequest request)
        {
            CreateStorageLocationIfNeed();

            var connection = Path.Combine(_configuration.StorageLocation, _configuration.StorageDbFileName);
            var id = string.IsNullOrWhiteSpace(request.Id) ? Guid.NewGuid().ToString("N") : request.Id;
            var imageId = $"{id}";

            using (var db = new LiteDatabase(connection))
            {
                var file = db.FileStorage.Upload(id, request.Name, request.Content);

                return new Protocol.File
                {
                    Id = imageId,
                    Name = file.Filename,
                    MimeType = file.MimeType
                };
            }
        }

        public File Load(LoadFileRequest request)
        {
            CreateStorageLocationIfNeed();

            var connection = Path.Combine(_configuration.StorageLocation, _configuration.StorageDbFileName);

            using (var db = new LiteDatabase(connection))
            {
                var file = db.FileStorage.FindById(request.Id);

                if (file != null)
                {
                    return new File
                    {
                        Id = request.Id,
                        Name = file.Filename,
                        MimeType = file.MimeType
                    };
                }

                return null;
            }
        }

        public Stream LoadContent(LoadFileRequest request)
        {
            CreateStorageLocationIfNeed();

            var connection = Path.Combine(_configuration.StorageLocation, _configuration.StorageDbFileName);

            using (var db = new LiteDatabase(connection))
            {
                if (db.FileStorage.Exists(request.Id))
                {
                    var stream = new MemoryStream();

                    db.FileStorage.Download(request.Id, stream);

                    stream.Position = 0;

                    return stream;
                }

                return null;
            }
        }

        private void CreateStorageLocationIfNeed()
        {
            if (!Directory.Exists(_configuration.StorageLocation))
            {
                Directory.CreateDirectory(_configuration.StorageLocation);
            }
        }
    }
}
