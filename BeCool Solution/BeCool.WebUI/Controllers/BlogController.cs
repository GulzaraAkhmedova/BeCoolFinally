using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BeCool.Application.Extensions;
using BeCool.Domain.Business.BlogPostModule;
using System.Threading.Tasks;
using BeCool.Domain.Models.Entities;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Models.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace BeCool.WebUI.Controllers
{
    public class BlogController : Controller
    {
        private readonly IMediator mediator;
        private readonly BeCoolDbContext db;


        public BlogController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index(BlogPostGetAllQuery query)
        {
            var response = await mediator.Send(query);

            if(Request.IsAjaxRequest())
            {
                return PartialView("_BlogPostsBody", response);
            }

            return View(response);
        }

        [AllowAnonymous]
        [Route("/blog/{slug}")]
        public async Task<IActionResult> Details(BlogPostSingleQuery query)
        {
            var blogPost = await mediator.Send(query);

            if (blogPost == null)
            {
                return NotFound();
            }

            return View(blogPost);
        }
        [HttpPost]
        [Route("/blog/postcomment")]
        public async Task<IActionResult> PostComment(int? commentId, int postId, string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
            {
                return Json(new
                {
                    error = true,
                    message = "Şərh boş buraxıla bilməz"
                });
            }
            if (postId < 1)
            {
                return Json(new
                {
                    error = true,
                    message = "Post mövcud deyil"
                });
            }


            var post = await db.BlogPosts.FirstOrDefaultAsync(bpc => bpc.Id == postId);

            if (post == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Post mövcud deyil"
                });
            }

            var commentModel = new BlogPostComment
            {
                BlogPostId = postId,
                Text = comment,
                CreatedByUserId = User.GetCurrentUserId()
            };

            if (commentId.HasValue && await db.BlogPostComments.AnyAsync(c => c.Id == commentId))
                commentModel.ParentId = commentId;

            db.BlogPostComments.Add(commentModel);
            await db.SaveChangesAsync();

            commentModel = await db.BlogPostComments
                .Include(c => c.CreatedByUser)
                .Include(c => c.Parent)
                .FirstOrDefaultAsync(c => c.Id == commentModel.Id);

            return PartialView("_Comment", commentModel);
        }
    }
}

