using BeCool.Application.Extensions;
using BeCool.Application.Infrastructure;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeCool.Domain.Business.ProductModule
{
    public class AddToBasketCommand : IRequest<JsonResponse>
    {
        public int ProductId { get; set; }

        public class AddToBasketCommandHandler : IRequestHandler<AddToBasketCommand, JsonResponse>
        {
            private readonly BeCoolDbContext db;
            private readonly IActionContextAccessor ctx;

            public AddToBasketCommandHandler(BeCoolDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(AddToBasketCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var alreadyExists = await db.Basket.AnyAsync(b => b.ProductId == request.ProductId && b.UserId == userId, cancellationToken);

                if (alreadyExists)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Sebetde movcuddur"
                    };
                }

                var basketItem = new Basket
                {
                    UserId = userId,
                    ProductId = request.ProductId,
                    Quantity = 1
                };

                await db.Basket.AddAsync(basketItem, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                var value = ctx.GetIntArrayFromCookie("favorites");

                if (value != null)
                {

                    ctx.SetValueToCookie("favorites", string.Join(",", value.Where(e => e != request.ProductId)));

                }

                return new JsonResponse
                {
                    Error = false,
                    Message = "Sebete elave edildi"
                };
            }
        }
    }
}
