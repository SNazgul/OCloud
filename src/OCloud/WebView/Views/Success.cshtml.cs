using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OCloud.WebView.Views
{
    [AllowAnonymous]
    public class SuccessModel : PageModel
    {
        public void OnGet(string operation)
        {
            Operation = operation;
            //ViewData["Operation"] = operation;
        }


        // [BindProperty(SupportsGet = true)] for case public void OnGet() 
        public string Operation { get; private set; }
    }
}