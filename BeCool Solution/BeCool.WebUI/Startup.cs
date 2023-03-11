using BeCool.Application.Providers;
using BeCool.Application.Services;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Models.DataContexts;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace BeCool.WebUI
{
    public class Startup
    {
        private readonly IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews(cfg =>
            {
                var policy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
                cfg.Filters.Add(new AuthorizeFilter(policy));

                cfg.ModelBinderProviders.Insert(0, new BooleanBinderProvider());
            });

            services.AddDbContext<BeCoolDbContext>(cfg =>
            {
                cfg.UseSqlServer(configuration["ConnectionStrings:cString"]);
            });


            services.SetupIdentity();

            services.AddAuthentication();

            services.AddAuthorization(cfg =>
            {
                foreach (var item in BeCool.Domain.AppCode.Extensions.Extension.policies)
                {
                    cfg.AddPolicy(item, p =>
                    {
                        //p.RequireClaim(item, "1");

                        p.RequireAssertion(ra =>
                        {
                            return ra.User.HasAccess(item);
                        });
                    });
                }

            });

            services.AddRouting(cfg =>
            {
                cfg.LowercaseUrls = true;
            });

            services.Configure<CryptoServiceOptions>(cfg =>
            {
                configuration.GetSection("cryptoGraphy").Bind(cfg);
            });
            services.AddSingleton<CryptoService>();

            services.Configure<EmailServiceOptions>(cfg =>
            {
                configuration.GetSection("emailAccount").Bind(cfg);
            });

            services.AddSingleton<EmailService>();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(assemblies => assemblies.FullName.StartsWith("BeCool."));
            services.AddMediatR(assemblies.ToArray());

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.SeedData();
            app.SeedMembership();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(cfg =>
            {
                //cfg.MapControllerRoute(
                //    name: "default",
                //    pattern: "{ controller = home}/{ action = index}/{id?}");

                cfg.MapAreaControllerRoute("defaultAdmin", "admin", "admin/{controller=dashboard}/{action=index}/{id?}");

                cfg.MapControllerRoute("default", "{controller=home}/{action=index}/{id?}");
            });
        }
    }
}
