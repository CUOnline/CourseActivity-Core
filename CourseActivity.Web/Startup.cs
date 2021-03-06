﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CourseActivity.BLL;
using CourseActivity.Interfaces.BLL;
using CourseActivity.Interfaces.Repository;
using CourseActivity.Repository;
using CourseActivity.Web.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CourseActivity.Web
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
            services.AddDbContext<CourseActivityContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            CanvasOAuth canvasOAuth = new CanvasOAuth();
            var canvasOAuthSection = Configuration.GetSection(nameof(CanvasOAuth));
            canvasOAuthSection.Bind(canvasOAuth);
            services.Configure<CanvasOAuth>(canvasOAuthSection);

            CanvasApiAuth apiAuth = new CanvasApiAuth();
            var canvasApiAuthSection = Configuration.GetSection(nameof(CanvasApiAuth));
            canvasApiAuthSection.Bind(apiAuth);
            services.Configure<CanvasApiAuth>(canvasApiAuthSection);

            services.AddHttpClient("CanvasClient", client =>
            {
                client.BaseAddress = new Uri(apiAuth.BaseUrl);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiAuth.ApiKey);
            });

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options =>
            {
                options.AccessDeniedPath = "/Home/AccessDenied";
                options.LoginPath = "/login";
                options.LogoutPath = "/signout";
            }).AddCanvas(options =>
            {
                options.UserInformationEndpoint = canvasOAuth.BaseUrl;
                options.AuthorizationEndpoint = canvasOAuth.AuthorizationEndpoint;
                options.TokenEndpoint = canvasOAuth.TokenEndpoint;
                options.ClientId = canvasOAuth.ClientId;
                options.ClientSecret = canvasOAuth.ClientSecret;
                options.ApiToken = apiAuth.ApiKey;
            });

            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // BLLs
            services.AddScoped<ICourseUploadBLL, CourseUploadBLL>();

            // Repository
            services.AddScoped<ICourseUploadRepository, CourseUploadRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private readonly string AllowSpecificOrigins = "_allowSpecificOrigins";

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
