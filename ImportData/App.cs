using ImportData.Models;
using ImportData.Services;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImportData
{
    public class App
    {
        private readonly IImportService _importService;
        private readonly ILogger<App> _logger;
        private readonly AppSettings _config;

        public App(IImportService testService,
            IOptions<AppSettings> config,
            ILogger<App> logger)
        {
            _importService = testService;
            _logger = logger;
            _config = config.Value;
        }

        public void Run()
        {
            _logger.LogInformation($"App Running");
            _importService.Run().Wait();
            System.Console.ReadKey();
        }
    }
}
