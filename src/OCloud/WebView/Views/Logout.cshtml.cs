using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OCloud.WebView.Views
{
    [Authorize(AuthenticationSchemes = "Identity.Application" + "," + CookieAuthenticationDefaults.AuthenticationScheme)]
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}