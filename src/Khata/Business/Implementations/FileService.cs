using System.IO;
using Business.Abstractions;
using Data.Core;

namespace Business.Implementations
{
    public class FileService : IFileService
    {
        private readonly IFileRepository _repo;
        public FileService(IFileRepository repo) 
            => _repo = repo;

        public Stream Get(string id) 
            => _repo.Get(id);

        public bool Save(string id, Stream file) 
            => _repo.Save(id, file);

        public bool Delete(string id) 
            => _repo.Delete(id);

        public bool Exists(string id)
            => _repo.Exists(id);
    }
}
