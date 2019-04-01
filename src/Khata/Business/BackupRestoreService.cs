using System.IO;
using System.Threading.Tasks;

using Data.Core;

namespace Business
{
    public class BackupRestoreService
    {
        private readonly IBackupRestoreRepository _repo;

        public BackupRestoreService(
            IBackupRestoreRepository repo)
        {
            _repo = repo;
        }

        public async Task<Stream> GetJsonDump()
        {
            return await _repo.GetJsonDump();
        }
    }
}
