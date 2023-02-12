using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using BeCool.Domain.AppCode.Providers;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities.Membership;
using System;
using System.Linq;
using System.Security.Claims;

namespace BeCool.Domain.AppCode.Extensions
{
    public static partial class Extension
    {

        public static string[] policies = null;

        public static IServiceCollection SetupIdentity(this IServiceCollection services)
        {

            

            services.AddIdentity<BeCoolUser, BeCoolRole>()
                .AddEntityFrameworkStores<BeCoolDbContext>();

            services.AddScoped<SignInManager<BeCoolUser>>();
            services.AddScoped<UserManager<BeCoolUser>>();
            services.AddScoped<RoleManager<BeCoolRole>>();


            services.AddScoped<IClaimsTransformation, AppClaimProvider>();

            services.Configure<IdentityOptions>(cfg =>
            {
                cfg.User.RequireUniqueEmail = true;
                //cfg.User.AllowedUserNameCharacters = "";
                cfg.Password.RequiredLength = 3;
                cfg.Password.RequireNonAlphanumeric = false;
                cfg.Password.RequireUppercase = false;
                cfg.Password.RequireLowercase = false;
                cfg.Password.RequireDigit = false;
                cfg.Password.RequiredUniqueChars = 1; //aabacc- tekrarlanmayan nece dene simvol olmalidi

                cfg.Lockout.DefaultLockoutTimeSpan = new TimeSpan(0,1,0);
                cfg.Lockout.AllowedForNewUsers = false;

                cfg.SignIn.RequireConfirmedPhoneNumber = false;
                cfg.SignIn.RequireConfirmedEmail = true;
                cfg.SignIn.RequireConfirmedAccount = false;
            });

            services.ConfigureApplicationCookie(cfg =>
            {
                cfg.LoginPath = "/signin.html";
                cfg.AccessDeniedPath = "/accessdenied.html";

                cfg.Cookie.Name = "BeCoolOil";
                cfg.Cookie.HttpOnly = true;
                cfg.ExpireTimeSpan = new TimeSpan(0,15,0);
            });

            return services;
        }

        public static bool HasAccess(this ClaimsPrincipal principal, string policyName)
        {
            if (principal.IsInRole("sa"))
            {
                return true;
            }
            return principal.Claims.Any(c => c.Type.Equals(policyName) && c.Value.Equals("1"));
        }
        public static int GetCurrentUserId(this ClaimsIdentity identity)
        {
            return Convert.ToInt32(
                identity.Claims.FirstOrDefault(c => 
                c.Type.Equals(ClaimTypes.NameIdentifier)).Value
                );
        }
        public static int GetCurrentUserId(this ClaimsPrincipal principal)
        {
            if (principal.Identity is ClaimsIdentity identity)
            {
                return identity.GetCurrentUserId();
            }
            return 0;
        }
        public static int GetCurrentUserId(this IActionContextAccessor ctx)
        {
            return ctx.ActionContext.HttpContext.User.GetCurrentUserId();
        }
    }
}
