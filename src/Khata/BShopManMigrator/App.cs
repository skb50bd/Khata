using ImportData.Models;
using ImportData.Services;

using Data.Core;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImportData
{
    public class App
    {
        private readonly ILogger<App> _logger;
        private readonly IUnitOfWork _db;
        private readonly AppSettings _config;

        public App(
            IOptions<AppSettings> config,
            IUnitOfWork db,
            ILogger<App> logger)
        {
            _db = db;
            _logger = logger;
            _config = config.Value;
        }

        public async System.Threading.Tasks.Task RunAsync()
        {
            _logger.LogInformation($"App Running");
            await Dump.InsertAllAsync(_db);

            System.Console.ReadKey();
        }
    }
}
