﻿using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using OCloud.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace OCloud.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : Controller
    {
        private IConfiguration _config;
        private readonly ILogger<AuthController> _logger;
        private UserManager<OCloudUser> _userManager;

        public AuthController(IConfiguration config, ILogger<AuthController> logger, UserManager<OCloudUser> userManager)
        {
            _config = config;
            _logger = logger;
            _userManager = userManager;
        }

        [AllowAnonymous]
        [HttpPost]
        [ActionName("login")]
        public async Task<IActionResult> CreateToken([FromBody]LoginModel login)
        {
            IActionResult response = Unauthorized();
            var user = await Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;
        }

        private string BuildToken(OCloudUser user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<OCloudUser> Authenticate(LoginModel login)
        {
            if (string.IsNullOrEmpty(login.Username) || string.IsNullOrEmpty(login.Password))
                return await Task.FromResult<OCloudUser>(null);

            OCloudUser user = await _userManager.FindByNameAsync(login.Username);
            bool? isPwdCorrect = null;
            if (user != null)
            {
                isPwdCorrect = await _userManager.CheckPasswordAsync(user, login.Password);
            }

            if (!isPwdCorrect.HasValue || !isPwdCorrect.Value)
            {
                if (Request.IsRequestFromLocalHost())
                {
                    if (login.Username.Equals(_config["LocalAdmin:username"]) &&
                        login.Password.Equals(_config["LocalAdmin:password"]))
                    {
                        user = new OCloudUser(_config["LocalAdmin:username"]);
                    }
                }
            }

            return user;
        }


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


        public class LoginModel
        {
            public string Username { get; set; }
            public string Password { get; set; }
            public override string ToString()
            {
                return $"Username: {Username}, pwd: {Password?.Length}";
            }
        }
    }
}