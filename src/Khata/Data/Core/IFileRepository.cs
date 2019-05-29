using System.IO;

namespace Data.Core
{
    public interface IFileRepository
    {
        Stream Get(string id);
        bool Save(string id, Stream file);
        bool Delete(string id);
        bool Exists(string id);
    }
}
