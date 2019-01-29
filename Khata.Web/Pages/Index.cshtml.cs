using Khata.Domain;

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        public OutletOptions Options { get; set; }
        public IndexModel(IOptionsMonitor<OutletOptions> options)
        {
            Options = options.CurrentValue;
        }
    }
}
