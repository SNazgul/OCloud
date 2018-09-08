using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCloud.Entities;
using OCloud.Utils;
using OCloud.WebView.Data;

namespace OCloud.WebView.Views
{
    [AllowAnonymous]
    public class LoginModel : PageModel
    {
        UserManager<OCloudUser> _userManager;


        public LoginModel(UserManager<OCloudUser> userManager)
        {
            LoginInfo = new LoginInfo { UserName = "user", Password = "pwd" };
            _userManager = userManager;
        }

        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            //ReturnUrl = returnUrl;

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await Authentification.AuthenticateUser(_userManager, LoginInfo.UserName, LoginInfo.Password);
            if (user == null)
            {
                ModelState.AddModelError("", "Invalid logging attempt");
                return Page();
            }


            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName)
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return LocalRedirect("/Success?op=Login");
        }


        [BindProperty]
        public LoginInfo LoginInfo { get; set; }
    }
}