using System.IO;
using System.Threading.Tasks;

namespace Data.Core
{
    public interface IBackupRestoreRepository
    {
        Task<Stream> GetJsonDump();
        Task<bool> RestoreFromJson(string dump);
    }
}
