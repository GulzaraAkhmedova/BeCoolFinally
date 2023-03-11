using BeCool.Application.Extensions;
using BeCool.Domain.Business.ProductModule;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BeCool.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private readonly IMediator mediator;

        public ShopController(IMediator mediator)
        {
            this.mediator = mediator;
        }
        [AllowAnonymous]
        public async Task<IActionResult> Index(ProductFilterQuery query)
        {
            var response = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                //return Json(response);
                return PartialView("_Products", response);
            }

            return View(response);
        }


        [AllowAnonymous]
        public async Task<IActionResult> Wishlist(WishlistQuery query)
        {
            var favs = await mediator.Send(query);

            if (Request.IsAjaxRequest())
            {
                return PartialView("_WishlistBody", favs);
            }

            return View(favs);
        }

        #region Basket Operations
        [Route("/basket")]
        //[Authorize(Policy = "shop.basket")]
        public async Task<IActionResult> Basket(ProductBasketQuery query)
        {
            var response = await mediator.Send(query);

            return View(response);
        }

        [HttpPost]
        [Route("/basket")]
        //[Authorize(Policy = "shop.basket")]
        public async Task<IActionResult> Basket(AddToBasketCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }

        [HttpPost]
        //[Route("/basket/{productId}")]
        //[Authorize(Policy = "shop.basket")]
        public async Task<IActionResult> RemoveFromBasket(RemoveFromBasketCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }

        [HttpPost]
        //[Route("/basket/{productId}")]
        //[Authorize(Policy = "shop.basket")]
        public async Task<IActionResult> ChangeBasketQuantity(ChangeBasketQuantityCommand command)
        {
            var response = await mediator.Send(command);
            return Json(response);
        }
        #endregion
    }
}
