using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using System.Net;
using Microsoft.Extensions.Logging;

namespace OCloud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IConfiguration _config;
        private readonly ILogger<AuthController> _logger;


        public AuthController(IConfiguration config, ILogger<AuthController> logger)
        {
            _config = config;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("login")]
        public IActionResult CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(UserModel user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            if (login.Username == "mario" && login.Password == "secret")
            {
                user = new UserModel { Name = "Mario Rossi", Email = "mario.rossi@domain.com" };
            }
            return user;
        }

        //public static bool IsLocal(this HttpRequest req)
        //{
        //    var connection = req.HttpContext.Connection;
        //    if (connection.RemoteIpAddress. .IsSet())
        //    {
        //        //We have a remote address set up
        //        return connection.LocalIpAddress. .IsSet()
        //            //Is local is same as remote, then we are local
        //            ? connection.RemoteIpAddress.Equals(connection.LocalIpAddress)
        //            //else we are remote if the remote IP address is not a loopback address
        //            : IPAddress.IsLoopback(connection.RemoteIpAddress);
        //    }

        //    return true;
        //}
        [AllowAnonymous]
        [HttpPost]
        [ActionName("register")]
        public IActionResult Register(LoginModel user)
        {
            _logger.LogTrace($"Enter: {nameof(Register)}");

            _logger.LogDebug($"CreateUser HttpContext.Connection.RemoteIpAddress = {HttpContext.Connection.RemoteIpAddress}");
            if (HttpContext.Connection.RemoteIpAddress != null)
                throw new UnauthorizedAccessException("It is forbidden to register a user from an external IP");



            return Ok();
        }


        //[AllowAnonymous]
        //[HttpGet]
        //[ActionName("register")]
        //public IActionResult Register()
        //{
        //    _logger.LogDebug($"D:CreateUser for ");
        //    _logger.LogTrace($"T:CreateUser for ");
        //    _logger.LogWarning($"W:CreateUser for ");

        //    _logger.LogDebug($"CreateUser HttpContext.Connection = {HttpContext.Connection}");
        //    if (HttpContext.Connection.RemoteIpAddress != null)
        //        return new UnauthorizedResult();

        //    return Ok();
        //}

        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public override string ToString()
            {
                return $"Username: {Username}, pwd: {Password?.Length}";
            }
        }

        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
        }
    }
}