using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCloud.Entities;


namespace OCloud.WebView.Views
{
    [AllowAnonymous]
    public class InitializeModel : PageModel
    {
        OCloudDbContext _cloudDbContext;

        public InitializeModel(OCloudDbContext cloudDbContext)
        {
            _cloudDbContext = cloudDbContext;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var local = Request.IsRequestFromLocalHost();
            if (!local)
            {
                TempData["MessageToUser"] = "'Initialize' can be done only from local host";
                return RedirectToPage("/PlainResult");
            }
            
            await _cloudDbContext.Database.EnsureDeletedAsync();
            IsInitialized = await _cloudDbContext.Database.EnsureCreatedAsync();
            return LocalRedirect("/Success/?operation=Infrastructure%20has%20been%20initialized%20successfully");
        }

        [BindProperty]
        public bool IsInitialized { get; set; }
    }
}