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
    public class ProductBasketQuery : IRequest<IEnumerable<Basket>>
    {

        public class ProductBasketQueryHandler : IRequestHandler<ProductBasketQuery, IEnumerable<Basket>>
        {
            private readonly BeCoolDbContext db;
            private readonly IActionContextAccessor ctx;

            public ProductBasketQueryHandler(BeCoolDbContext db, IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<IEnumerable<Basket>> Handle(ProductBasketQuery request, CancellationToken cancellationToken)
            {
                if (!ctx.ActionContext.HttpContext.User.Identity.IsAuthenticated)
                {
                    return Enumerable.Empty<Basket>();
                }

                var userId = ctx.GetCurrentUserId();

                var data = await db.Basket
                    .Include(b => b.Product)
                    .ThenInclude(p => p.Images.Where(i => i.IsMain && i.DeletedUserId == null))

                    .Where(b => b.UserId == userId)
                    .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
