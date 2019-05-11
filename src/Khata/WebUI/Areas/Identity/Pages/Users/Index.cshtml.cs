using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Areas.Identity.Pages.Users
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public IndexModel(
            UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IList<ApplicationUser> AppUsers;

        public async Task<IActionResult> OnGetAsync()
        {
            AppUsers = await _userManager.Users.ToListAsync();

            var defaultAdmin =
                AppUsers.FirstOrDefault(
                    u => u.UserName == "brotal");

            AppUsers.Remove(defaultAdmin);

            return Page();
        }
    }
}