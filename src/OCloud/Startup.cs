using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using OCloud.Entities;
using OCloud.Exceptions;


namespace OCloud
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var value = Configuration["Desription"];

            services.AddAuthentication(/*JwtBearerDefaults.AuthenticationScheme*/)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });

            // If you don't want the cookie to be automatically authenticated and assigned to HttpContext.User, 
            // remove the CookieAuthenticationDefaults.AuthenticationScheme parameter passed to AddAuthentication.
            services.AddAuthentication(/*CookieAuthenticationDefaults.AuthenticationScheme*/)
                    .AddCookie(options =>
                    {
                        options.LoginPath = "/Login";
                        options.LogoutPath = "/Logout";
                    });


            //services.AddAuthorization(options =>
            //    options.AddPolicy("JwtAuthPolicy", policy =>
            //    {
            //        policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
            //    })
            //);

            //services.AddAuthorization(options =>
            //    options.AddPolicy("CookieAuthPolicy", policy =>
            //    {
            //        policy.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
            //    })
            //);

#if DEBUG
            String connectionStr = String.Format(Configuration["database:connection"], Configuration["database:development:ServerIP"]);
#else
            String connectionStr = String.Format(Configuration["database:connection"], Configuration["database:development:LocalAddress"]);
            Console.WriteLine(connectionStr);
#endif


            //services.AddDbContext<OCloudDbContext>(options =>
            //    options.UseMySQL(connectionStr));
            services.AddDbContext<OCloudDbContext>(options =>
                options.UseNpgsql(connectionStr/*Configuration.GetConnectionString("DefaultConnection")*/));
            services.AddIdentity<OCloudUser, IdentityRole>()
                            .AddEntityFrameworkStores<OCloudDbContext>()
                            .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

                options.LoginPath = "/Login";
                options.AccessDeniedPath = "/AccessDenied";
                options.SlidingExpiration = true;
            });

            services.AddMvc(config =>
                {
                    // Set the default authentication policy to require users to be authenticated. 
                    var policy = new AuthorizationPolicyBuilder()
                                     .RequireAuthenticatedUser()
                                     .Build();
                    config.Filters.Add(new AuthorizeFilter(policy));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddRazorPagesOptions(options =>
                {
                    options.RootDirectory = @"/WebView/Views";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();                
                app.UseExceptionMiddleware();
            }
            else
            {                
                app.UseHsts();
            }

            //app.UseHttpsRedirection();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
