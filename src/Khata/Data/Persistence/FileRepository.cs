using System;
using System.IO;
using Data.Core;
using LiteDB;
using Microsoft.Extensions.Configuration;

namespace Data.Persistence
{
    public class FileRepository : IFileRepository
    {
        private readonly string _connectionString;

        public FileRepository(IConfiguration config)
        {
            _connectionString =
                config.GetConnectionString(
                "LiteDB");
        }

        public Stream Get(string id)
        {
            if (!Exists(id))
                throw new Exception(
                    "File doesn't exist.");

            var repo = new LiteRepository(_connectionString);
            var file = repo.FileStorage.FindById(id);

            var stream = new MemoryStream();
            file.CopyTo(stream);
            stream.Position = 0;

            repo.Dispose();
            return stream;
        }

        public bool Exists(string id)
        {
            bool exists;
            using (var repo =
                new LiteRepository(_connectionString))
                exists = repo.FileStorage.Exists(id);
            return exists;
        }

        public bool Save(string id, Stream file)
        {
            if (file.Length == 0)
                return false;

            file.Position = 0;

            using (var repo =
                new LiteRepository(_connectionString))
                repo.FileStorage
                    .Upload(id, id, file);
            file.Close();
            file.Dispose();

            return true;
        }

        public bool Delete(string id)
        {
            if (!Exists(id))
                return false;

            using (var repo =
                new LiteRepository(_connectionString))
                repo.FileStorage.Delete(id);

            return true;
        }
    }
}
