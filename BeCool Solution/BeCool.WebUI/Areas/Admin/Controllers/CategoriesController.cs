using BeCool.Domain.Business.CategoryModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly IMediator mediator;

        public CategoriesController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [Authorize(Policy = "admin.categories.index")]
        public async Task<IActionResult> Index(CategoriesPagedQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }
    }
}
