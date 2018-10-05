using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OCloud.Entities;

namespace OCloud.WebView.Views
{
    [AllowAnonymous]
    public class EraseAllModel : PageModel
    {
        OCloudDbContext _cloudDbContext;

        public EraseAllModel(OCloudDbContext cloudDbContext)
        {
            _cloudDbContext = cloudDbContext;
        }

        public void OnGet()
        {
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            bool bRes = await _cloudDbContext.Database.EnsureDeletedAsync();
            TempData["MessageToUser"] = bRes ? "Delete succesfully" : "Data base doesn't exist";
            return RedirectToPage("/PlainResult");
        }
    }
}