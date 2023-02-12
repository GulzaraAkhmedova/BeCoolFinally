using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BeCool.Domain.Models.Entities.Membership;
using BeCool.Domain.Models.FormModels;
using System.Threading.Tasks;
using BeCool.Application.Services;
using BeCool.Application.Extensions;

namespace BeCool.WebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<BeCoolUser> signInManager;
        private readonly UserManager<BeCoolUser> userManager;
        private readonly EmailService emailService;

        public AccountController(SignInManager<BeCoolUser> signInManager,
            UserManager<BeCoolUser> userManager, EmailService emailService)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.emailService = emailService;
        }

        [AllowAnonymous]
        [Route("/signin.html")]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("/signin.html")]

        public async Task<IActionResult> Signin(UserFormModel user)
        {
            if (ModelState.IsValid)
            {
                BeCoolUser foundedUser = null;

                if (user.UserName.IsEmail())
                {
                    foundedUser = await userManager.FindByEmailAsync(user.UserName);
                }
                else
                {
                    foundedUser = await userManager.FindByNameAsync(user.UserName);
                }

                if (foundedUser == null)
                {
                    ViewBag.Message = "İstifadəçi adınız vəya şifrəniz yanlışdır";
                    goto end;
                }

                var signInResult = await signInManager.PasswordSignInAsync(foundedUser, user.Password, true, true); //qalici 1-ci true-qalici olaraq yaddasda saxlasin-2-ci true defelerle sehv edende bloklasin...

                if (!signInResult.Succeeded)
                {
                    ViewBag.Message = "İstifadəçi adınız vəya şifrəniz yanlışdır";
                    goto end;
                }

                var redirectUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrWhiteSpace(redirectUrl))
                {
                    return Redirect(redirectUrl);
                }

                return RedirectToAction("Index", "Home");
            }

        end:
            return View(user);
        }




        [AllowAnonymous]
        [Route("/register.html")]
        public IActionResult Register()
        {
            return View();
        }



        [HttpPost]
        [AllowAnonymous]
        [Route("/register.html")]
        public async Task<IActionResult> Register(RegisterFormModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new BeCoolUser();
                user.Email = model.Email;
                user.UserName = model.Email;
                user.Name = model.Name;
                user.Surname = model.Surname;
                //user.EmailConfirmed = true;



                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    string message = $"Qeydiyyatı <a href='https://{Request.Host}/registration-confirm.html?email={user.Email}token={token}'>link</a> vasitesile tesdiq edin";

                    var emailResponse = await emailService.SendEmailAsync(user.Email, "MotorOil user registration", message);



                    if (emailResponse)
                    {
                        ViewBag.Message = "Qeydiyyat tamamlandı";
                    }
                    else
                    {
                        ViewBag.Message = "Yenidən cəhd edin";
                    }

                    //ama tam deyil email confirm link gonderilmelidi


                    return RedirectToAction(nameof(SignIn));
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }
            }
            return View(model);
        }


        [AllowAnonymous]
        [Route("/registration-confirm.html")]
        public async Task<IActionResult> RegisterConfirm(string email, string token)
        {
            var foundedUser = await userManager.FindByEmailAsync(email);
            if (foundedUser == null)
            {
                ViewBag.Message = "Xətalı token";
                goto end;
            }

            token = token.Replace(" ", "+");

            var result = await userManager.ConfirmEmailAsync(foundedUser, token);

            if (!result.Succeeded)
            {
                ViewBag.Message = "Xətalı token";
                goto end;
            }

            ViewBag.Message = "Hesabınız təsdiqləndi";

        end:
            return RedirectToAction(nameof(SignIn));
        }

        [Route("/logout.html")]
        public async Task<IActionResult> LogOut()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("index", "home");
        }
    }
}

