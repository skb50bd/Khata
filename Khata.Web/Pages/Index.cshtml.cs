using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace Khata.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration Config;
        public IndexModel(IConfiguration config)
        {
            Config = config;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }


        public void OnGet()
        {
            Title = Config.GetValue<string>("OutletInfo:Title");
            ShortDescription = Config.GetValue<string>("OutletInfo:ShortDescription");
        }
    }
}
