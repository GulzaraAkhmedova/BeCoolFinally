using BeCool.Application.Extensions;
using BeCool.Application.Services;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using BeCool.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BeCool.WebUI.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly BeCoolDbContext db;
        private readonly CryptoService crypto;
        private readonly EmailService emailService;

        public HomeController(BeCoolDbContext db, CryptoService crypto, EmailService emailService)
        {
            this.db = db;
            this.crypto = crypto;
            this.emailService = emailService;
        }
        
        public async Task<IActionResult> Index()
        {
            HomeVM vM = new()
            {
                BlogPosts = await db.BlogPosts.Where(x => x.DeletedDate == null).ToListAsync(),
                Products = await db.Products.Include(x => x.Images).Where(x => x.DeletedDate == null).ToListAsync()
            };
            return View(vM);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Contact(ContactPost model)
        {
            if (ModelState.IsValid)
            {
                db.ContactPosts.Add(model);
                db.SaveChanges();

                //ViewBag.Message = "Muracietiniz qebul olundu";

                //ModelState.Clear();
                //return View();
                var response = new
                {
                    error = false,
                    message = "Muracietiniz qebul olundu"

                };
                return Json(response);
            }
            var responseError = new
            {
                error = true,
                message = "Melumatlar uygun deyil",
                state = ModelState.GetErrors()
            };
            return Json(responseError);
        }
        public IActionResult Faq()
        {
            var data = db.Faqs.Where(f => f.DeletedDate == null).ToList();
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(Subscribe model)
        {
            if (!ModelState.IsValid)
            {
                string msg = ModelState.Values.First().Errors[0].ErrorMessage;
                return Json(new
                {
                    error = true,
                    message = msg
                });
            }
            var entity = db.Subscribes.FirstOrDefault(s => s.Email.Equals(model.Email));

            if (entity != null && entity.IsApproved == true)
            {
                return Json(new
                {
                    error = false,
                    message = "Siz artiq abune olmusunuz"
                });
            }

            if (entity == null)
            {
                db.Subscribes.Add(model);
                db.SaveChanges();
            }
            else if(entity != null)
            {
                model.Id = entity.Id;
            }


            string token = $"{model.Id}-{model.Email}-{Guid.NewGuid()}";

            token = crypto.Encrypt(token,true);

            //token = HttpUtility.UrlEncode(token);

            

            string message = $"Abuneliyinizi <a href='https://{Request.Host}/approve-subscribe?token={token}'>link</a> vasitesile tesdiq edin";


            //configuration.SendMail(model.Email, message, "Subscribe Approve Ticket");

            await emailService.SendEmailAsync(model.Email,"Subscribe Approve ticket",message);

            return Json(new
            {
                error = false,
                message = "Email təsdiq mətni göndərildi"
            });

        }


        //   /approve-subscribe? token = 2
        [HttpGet] // bu
        [Route("/approve-subscribe")]
        //public string SubscribeApprove(string token)
        public IActionResult SubscribeApprove(string token) //bu
        {

            token = crypto.Decrypt(token);
            Match match = Regex.Match(token, @"^(?<id>\d+)-(?<email>[^-]+)-(?<randomKey>.*)$");
            if (match.Success)
            {
                int id = Convert.ToInt32(match.Groups["id"].Value);
                string email = match.Groups["email"].Value;
                string randomKey = match.Groups["randomKey"].Value;

                var entity = db.Subscribes.FirstOrDefault(s => s.Id == id);

                if (entity == null)
                {
                    ViewBag.Message = Tuple.Create(true, "Token uygun deyil");
                    goto end;
                }

                if (entity.IsApproved == true)
                {
                    ViewBag.Message = Tuple.Create(true, "Artiq tesdiq edilib");
                    goto end;
                }

                entity.IsApproved = true;
                entity.ApprovedDate = DateTime.UtcNow.AddHours(4);
                db.SaveChanges();

                ViewBag.Message = Tuple.Create(false, "Abuneliyiniz tesdiq edildi");

                //return Ok(new
                //{
                //    id = id,
                //    email = email,
                //    randomKey = randomKey
                //});
            }
            else
            {
                ViewBag.Message = Tuple.Create(true, "Token xətası");
                goto end;
            }
            end:
            return View();


            //token = token.Decrypt(Program.key);

            //Match match = Regex.Match(token, @"^(?<id>\d+)-(?<email>[^-]+)-(?<randomKey>.*)$");

            //if(!match.Success)
            //{
            //    return "token uygun deyil";
            //}


            //int id = Convert.ToInt32(match.Groups["id"].Value);
            //string email = match.Groups["email"].Value;
            //string randomKey = match.Groups["randomKey"].Value;

            //var entity = db.Subscribes.FirstOrDefault(s => s.Id == id);

            //if (entity == null)
            //{
            //    return "Tapilmadi";
            //}

            //if (entity.IsApproved)
            //{
            //    return "Artiq tesdiq edilib";
            //}

            //entity.IsApproved = true;
            //entity.ApprovedDate = DateTime.UtcNow.AddHours(4);
            //db.SaveChanges();

            //return $"Id: {id} | Email: {email} | RandomKey: {randomKey}";
        }

    }
}
