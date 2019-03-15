using System;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using Brotal.Extensions;
using Newtonsoft.Json;

namespace WebUI.Pages
{
    public class IndexModel : PageModel
    {
        public OutletOptions Options { get; set; }
        public IndexModel(
            IOptionsMonitor<OutletOptions> options)
        {
            Options = options.CurrentValue;
        }
    }
}
