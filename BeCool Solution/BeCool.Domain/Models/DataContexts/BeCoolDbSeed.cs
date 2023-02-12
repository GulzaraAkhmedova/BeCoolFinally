using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using BeCool.Domain.Models.Entities;
using BeCool.Domain.Models.Entities.Membership;
using System;
using System.Linq;

namespace BeCool.Domain.Models.DataContexts
{
    public static class BeCoolDbSeed
    {

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<BeCoolDbContext>();

                db.Database.Migrate(); //update-database

                InitBrands(db);
            }
            return app;
        }

        public static IApplicationBuilder SeedMembership(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var signInManager = scope.ServiceProvider.GetRequiredService<SignInManager<BeCoolUser>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<BeCoolUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<BeCoolRole>>();

                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();

                string superAdminRoleName = configuration["defaultAccount:superAdmin"];
                string superAdminEmail = configuration["defaultAccount:email"];
                string superAdminUserName = configuration["defaultAccount:username"];
                string superAdminPassword = configuration["defaultAccount:password"];


                var superAdminRole = roleManager.FindByNameAsync(superAdminRoleName).Result;

                if (superAdminRole == null)
                {
                    superAdminRole = new BeCoolRole
                    {
                        Name = superAdminRoleName
                    };

                    var roleResult = roleManager.CreateAsync(superAdminRole).Result;

                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Problem at Role Creating.....");
                    }
                }

                var superAdminUser = userManager.FindByEmailAsync(superAdminEmail).Result;

                if (superAdminUser == null)
                {
                    superAdminUser = new BeCoolUser
                    {
                        Email = superAdminEmail,
                        UserName = superAdminUserName,
                        EmailConfirmed = true
                    };

                    var userResult = userManager.CreateAsync(superAdminUser,superAdminPassword).Result;

                    if (!userResult.Succeeded)
                    {
                        throw new Exception("Problem at User Creating.....");
                    }
                }
                var isInRole = userManager.IsInRoleAsync(superAdminUser, superAdminRole.Name).Result;

                if (isInRole != true)
                {
                    userManager.AddToRoleAsync(superAdminUser, superAdminRole.Name).Wait();
                }
                
            }
            return app;
        }

        private static void InitBrands(BeCoolDbContext db)
        {
            if (!db.Brands.Any())
            {
                db.Brands.Add(new Brand
                {
                    Name = "Nike"
                });
                db.Brands.Add(new Brand
                {
                    Name = "Adidas"
                });
                db.SaveChanges();
            }
        }
    }
}
