using System.IO;

namespace Business.Abstractions
{
    public interface IFileService
    {
        Stream Get(string  id);
        bool Save(string id, Stream file);
        bool Delete(string id);
        bool Exists(string id);
    }
}
