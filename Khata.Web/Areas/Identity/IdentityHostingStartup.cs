using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(Khata.Web.Areas.Identity.IdentityHostingStartup))]
namespace Khata.Web.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}