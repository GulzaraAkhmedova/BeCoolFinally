using BeCool.Application.Infrastructure;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Models.DataContexts;
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

    public class RemoveFromBasketCommand : IRequest<JsonResponse>
    {
        public int ProductId { get; set; }

        public class RemoveFromBasketCommandHandler : IRequestHandler<RemoveFromBasketCommand, JsonResponse>
        {
            private readonly BeCoolDbContext db;
            private readonly IActionContextAccessor ctx;

            public RemoveFromBasketCommandHandler(BeCoolDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<JsonResponse> Handle(RemoveFromBasketCommand request, CancellationToken cancellationToken)
            {
                var userId = ctx.GetCurrentUserId();

                var basketItem = await db.Basket.FirstOrDefaultAsync(b => b.ProductId == request.ProductId
                                                && b.UserId == userId, cancellationToken);

                if (basketItem == null)
                {
                    return new JsonResponse
                    {
                        Error = true,
                        Message = "Sebetde movcud deyil"
                    };
                }

                db.Basket.Remove(basketItem);
                await db.SaveChangesAsync(cancellationToken);

                //var info = (await db.Basket
                //    .Include(b => b.Product)
                //    .Where(b => b.UserId == userId)
                //    .GroupBy(g => g.UserId)
                //    .FirstOrDefaultAsync(cancellationToken))
                //    ?.
                //    ;
                var info = await (from b in db.Basket
                                  join p in db.Products on b.ProductId equals p.Id
                                  where b.UserId == userId
                                  select new
                                  {
                                      b.UserId,
                                      SubTotal = p.Price * b.Quantity
                                  })
                                  .GroupBy(g => g.UserId)
                                  .Select(g => new
                                  {
                                      Total = g.Sum(m => m.SubTotal),
                                      Count = g.Count()
                                  })
                                  .FirstOrDefaultAsync(cancellationToken);

                return new JsonResponse
                {
                    Error = false,
                    Message = "Sebetden silindi",
                    Value = info
                };
            }

        }
    }
}
