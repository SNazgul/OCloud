using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCloud.WebView.Data;


namespace OCloud.WebView.Views
{
    //[Authorize]
    [AllowAnonymous]
    public class CreateUserModel : PageModel
    {
        
        public void OnGet()
        {
        }

        [BindProperty]
        public LoginInfo LoginInfo { get; set; }
    }
}