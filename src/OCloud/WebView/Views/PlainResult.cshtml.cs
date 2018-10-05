using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace OCloud.WebView.Views
{
    [AllowAnonymous]
    public class PlainResultModel : PageModel
    {
        public PlainResultModel()
        {
            Result = "sss";
        }

        public void OnGet()
        {
            Result = TempData["MessageToUser"] as string;
        }

        [BindProperty(SupportsGet = true)]
        public string Result { get; set; }
    }
}