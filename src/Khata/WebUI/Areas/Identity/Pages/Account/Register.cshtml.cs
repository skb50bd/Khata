using System;
using System.IO;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Business.Abstractions;
using Domain;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebUI.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public partial class RegisterModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IFileService    _fs;
        private readonly IImageProcessor _ip;

        public RegisterModel(
            UserManager<User> userManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            IFileService fs, 
            IImageProcessor ip)
        {
            _userManager = userManager;
            _logger = logger;
            _emailSender = emailSender;
            _fs = fs;
            _ip = ip;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if (ModelState.IsValid)
            {
                var user = new User
                {
                    FirstName = Input.FirstName,
                    LastName = Input.LastName,
                    UserName = Input.Username,
                    Email = Input.Email
                };

                if (Input.Avatar?.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await Input.Avatar.CopyToAsync(memoryStream);
                        var ext      = Path.GetExtension(Input.Avatar.FileName);
                        var fileName = Guid.NewGuid() + ext;

                        var resizedImage =
                            _ip.Resize(
                                memoryStream,
                                300,
                                300);

                        _fs.Save(fileName, resizedImage);
                        user.Avatar = fileName;
                    }
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { userId = user.Id, code },
                        protocol: Request.Scheme);

                    await _userManager.AddToRoleAsync(user, user.Role.ToString());

                    await _emailSender.SendEmailAsync(
                        Input.Email, 
                        "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    // await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
