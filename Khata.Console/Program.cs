using Microsoft.Extensions.DependencyInjection;

namespace Khata.Console
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // create service collection
            var serviceCollection = new ServiceCollection();
            serviceCollection.ConfigureServices();

            // create service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // run app
            serviceProvider.GetService<App>().Run();
        }
    }
}
