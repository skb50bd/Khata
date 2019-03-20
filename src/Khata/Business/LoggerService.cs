
using Microsoft.Extensions.Configuration;

using Serilog;

namespace Business
{
    public static class LoggerService
    {
        public static void CreateLogger(IConfiguration conf)
        {
            Log.Logger = new LoggerConfiguration()
                        .ReadFrom.Configuration(conf)
                        .CreateLogger();
        }
    }
}
