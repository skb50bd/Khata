using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using BackupRestore.Models;
using BackupRestore.Services;

namespace BackupRestore
{
    public class App
    {
        private readonly IBackupRestoreService _brService;
        private readonly AppSettings _config;

        public App(IBackupRestoreService brService,
            IOptions<AppSettings> config)
        {
            _brService = brService;
            _config = config.Value;
        }

        public void Run()
        {
            _brService.Run();
            System.Console.ReadKey();
        }
    }
}
