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
    public class InitializeModel : PageModel
    {
        OCloudDbContext _cloudDbContext;

        public InitializeModel(OCloudDbContext cloudDbContext)
        {
            _cloudDbContext = cloudDbContext;
        }

        public async Task OnGetAsync()
        {

            await _cloudDbContext.Database.EnsureDeletedAsync();
            IsInitialized = await _cloudDbContext.Database.EnsureCreatedAsync();
        }

        [BindProperty]
        public bool IsInitialized { get; set; }
    }
}