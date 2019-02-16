using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebUI.Pages.Reporting
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public string FromText { get; set; }
        [BindProperty]
        public string ToText { get; set; }
        public void OnGet()
        {

        }
    }
}