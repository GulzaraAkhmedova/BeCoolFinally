using BeCool.Application.Extensions;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities.Membership;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Extension = BeCool.Domain.AppCode.Extensions.Extension;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly BeCoolDbContext db;

        public UsersController(BeCoolDbContext db)
        {
            this.db = db;
        }
        [Authorize("admin.users.index")]
        public async Task<IActionResult> Index()
        {
            var data = await db.Users.ToListAsync();
            return View(data);
        }
        [Authorize("admin.users.details")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await db.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            ViewBag.Roles = await (from r in db.Roles
                                   join ur in db.UserRoles on new { RoleId = r.Id, UserId = user.Id } equals new { ur.RoleId, ur.UserId } into lJoin
                                   from lj in lJoin.DefaultIfEmpty()
                                   select Tuple.Create(r.Id, r.Name, lj != null)).ToListAsync();

            ViewBag.Policies = (from p in Extension.policies
                                join uc in db.UserClaims on new { ClaimValue = "1", ClaimType = p, UserId = user.Id } equals new { uc.ClaimValue, uc.ClaimType, uc.UserId } into lJoin
                                from lj in lJoin.DefaultIfEmpty()
                                select Tuple.Create(p, lj != null)).ToList();

            return View(user);
        }
        [HttpPost]
        [Route("/user-set-role")]
        [Authorize("admin.users.setrole")]
        public async Task<IActionResult> SetRole(int userId, int roleId, bool selected)
        {
            #region Check User and Role
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }

            if (userId == User.GetCurrentUserId())
            {
                return Json(new
                {
                    error = true,
                    message = "İstifadəçi özü özünü səlahiyyətləndirə bilməz"
                });
            }


            var role = await db.Roles.FirstOrDefaultAsync(r => r.Id == roleId);


            if (role == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }
            #endregion


            if (selected)
            {
                if (await db.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{role.Name}' adlı roldadır"
                    });
                }
                else
                {
                    db.UserRoles.Add(new BeCoolUserRole
                    {
                        UserId = userId,
                        RoleId = roleId
                    });

                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{role.Name}' rola əlavə edildi"
                    });
                }
            }
            else
            {
                var userRole = await db.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (userRole == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{role.Name}' adlı rolda deyil!"
                    });
                }
                else
                {
                    db.UserRoles.Remove(userRole);

                    await db.SaveChangesAsync();
                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{role.Name}' roldan çıxarıldı"
                    });
                }
            }
        }

        [HttpPost]
        [Route("/user-set-policy")]
        [Authorize("admin.users.setpolicy")]
        public async Task<IActionResult> SetPolicy(int userId, string policyName, bool selected)
        {
            #region Check User and Policy
            var user = await db.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }



            if (userId == User.GetCurrentUserId())
            {
                return Json(new
                {
                    error = true,
                    message = "İstifadəçi özü özünü səlahiyyətləndirə bilməz"
                });
            }

            var hasPolicy = Extension.policies.Contains(policyName);
            if (!hasPolicy)
            {
                return Json(new
                {
                    error = true,
                    message = "Xətalı sorğu"
                });
            }
            #endregion

            if (selected)
            {
                if (await db.UserClaims.AnyAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(policyName) && uc.ClaimValue.Equals("1")))
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{policyName}' adlı səlahiyyətə malikdir"
                    });
                }
                else
                {
                    db.UserClaims.Add(new BeCoolUserClaim
                    {
                        UserId = userId,
                        ClaimType = policyName,
                        ClaimValue = "1"
                    });
                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.Surname}' istifadəçiyə '{policyName}' səlahiyyəti verildi"
                    });
                }
            }
            else
            {
                var userClaim = await db.UserClaims.FirstOrDefaultAsync(uc => uc.UserId == userId && uc.ClaimType.Equals(policyName) && uc.ClaimValue.Equals("1"));
                if (userClaim == null)
                {
                    return Json(new
                    {
                        error = true,
                        message = $"'{user.Name} {user.Surname}' adlı istifadəçi '{policyName}' adlı səlahiyyətə malik deyil"
                    });
                }
                else
                {
                    db.UserClaims.Remove(userClaim);
                    await db.SaveChangesAsync();

                    return Json(new
                    {
                        error = false,
                        message = $"'{user.Name} {user.Surname}' istifadəçidən '{policyName}' səlahiyyəti çıxarıldı"
                    });
                }
            }
        }
    }
}
