using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace OCloud.Exceptions
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class HandleExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandleExceptionMiddleware> _logger;

        public HandleExceptionMiddleware(RequestDelegate next, ILogger<HandleExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                if (httpContext.Response.HasStarted)
                {
                    _logger.LogWarning($"The response has already started, the http status code middleware will not be executed for exception type {ex.GetType().ToString()} ({ex.ToString()}).");
                    throw;
                }

                await HandleExceptionAsync(httpContext, ex);
            }   
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var code = HttpStatusCode.InternalServerError;  // 500 if unexpected
            string[] descriptions = null;

            if (exception is System.Security.Authentication.AuthenticationException)
            {
                code = HttpStatusCode.Unauthorized;
                descriptions = new string[1] { exception.Message };
            }
            //else if (exception is MyUnauthorizedException) code = HttpStatusCode.Unauthorized;
            //else if (exception is MyException) code = HttpStatusCode.BadRequest;


            var result = JsonConvert.SerializeObject(new { Code = code, Descriptions = descriptions });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class HandleExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<HandleExceptionMiddleware>();
        }
    }
}
