using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using OCloud.Entities;
using OCloud.WebView.Data;
using System.Threading.Tasks;

namespace OCloud.WebView.Views
{
    //[Authorize]
    // allowed only from local host
    [AllowAnonymous]
    public class CreateUserModel : PageModel
    {
        private readonly UserManager<OCloudUser> _userManager;
        private readonly ILogger<CreateUserModel> _logger;
        private readonly SignInManager<OCloudUser> _signInManager;


        public CreateUserModel(ILogger<CreateUserModel> logger, UserManager<OCloudUser> userManager, SignInManager<OCloudUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult OnGet()
        {
            if (!Request.IsRequestFromLocalHost())
            {
                return LocalRedirect("/AccessDenied/?Reason=Creation%20is%20allowed%20from%20localhost%20only");
            }

            return Page(); 
        }

        [BindProperty]
        public LoginInfo LoginInfo { get; set; }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = new OCloudUser(LoginInfo.UserName);
               
                var result = await _userManager.CreateAsync(user, LoginInfo.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation($"User '{LoginInfo.UserName}' created a new account with password.");

                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Page(
                    //    "/Account/ConfirmEmail",
                    //    pageHandler: null,
                    //    values: new { userId = user.Id, code = code },
                    //    protocol: Request.Scheme);

                    //await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                    //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (LoginInfo.LoginAfterCreation)
                        await _signInManager.SignInAsync(user, isPersistent: false);
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