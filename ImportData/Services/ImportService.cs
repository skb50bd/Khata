
using System.Threading.Tasks;

using ImportData.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ImportData.Services
{
    public interface IImportService
    {
        Task Run();
    }

    public class ImportService : IImportService
    {
        private readonly ILogger<IImportService> _logger;
        private readonly AppSettings _config;
        private readonly Import _import;

        public ImportService(ILogger<IImportService> logger,
            Import import,
            IOptions<AppSettings> config)
        {
            _logger = logger;
            _import = import;
            _config = config.Value;
        }


        public async Task Run()
        {
            await _import.Info();
        }
        protected static string RemoveBsonNull(string str) => str == "BsonNull" ? "" : str;

    }
}
