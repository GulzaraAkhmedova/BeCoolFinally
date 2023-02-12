using MediatR;
using Microsoft.AspNetCore.Mvc;
using BeCool.Domain.Business.BlogPostModule;
using System.Threading.Tasks;

namespace BeCool.WebUI.AppCode.ViewComponents
{
    public class RelatedPostsViewComponent : ViewComponent
    {
        private readonly IMediator mediator;

        public RelatedPostsViewComponent(IMediator mediator)
        {
            this.mediator = mediator;
        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            var query = new BlogPostRelatedQuery() { Size = 4, PostId = id };
            var posts = await mediator.Send(query);
            return View(posts);
        }
    }
}
