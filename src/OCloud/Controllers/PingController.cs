using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OCloud.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class PingController : Controller
    {
        // GET api/ping
        [HttpGet]
        public string Get()
        {
            return "Pong";
        }
    }
}