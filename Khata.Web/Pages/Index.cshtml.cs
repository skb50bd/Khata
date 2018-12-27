using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _config;
        public IndexModel(IConfiguration config)
        {
            _config = config;
        }

        public string Title { get; set; }
        public string ShortDescription { get; set; }


        public void OnGet()
        {
            Title = _config.GetValue<string>("OutletInfo:Title");
            ShortDescription = _config.GetValue<string>("OutletInfo:ShortDescription");
        }
    }
}
