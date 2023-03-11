using BeCool.Application.Infrastructure;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeCool.Domain.Business.ProductModule
{
    public class ProductFilterQuery : PaginateModel, IRequest<PagedViewModel<Product>>
    {
        public int[] Materials { get; set; }
        public int[] Sizes { get; set; }
        public int[] Types { get; set; }
        public int[] Brands { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        public class ProductFilterQueryHandler : IRequestHandler<ProductFilterQuery, PagedViewModel<Product>>
        {
            private readonly BeCoolDbContext db;

            public ProductFilterQueryHandler(BeCoolDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<Product>> Handle(ProductFilterQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 12)
                {
                    request.PageSize = 12;
                }

                var query = db.ProductCatalog.AsQueryable();


                if (request.Materials != null && request.Materials.Length > 0)
                {
                    query = query.Where(q => request.Materials.Contains(q.ProductMaterialId));
                }

                if (request.Sizes != null && request.Sizes.Length > 0)
                {
                    query = query.Where(q => request.Sizes.Contains(q.ProductSizeId));
                }

                if (request.Types != null && request.Types.Length > 0)
                {
                    query = query.Where(q => request.Types.Contains(q.ProductTypeId));
                }



                var productIds = await query.Select(q => q.ProductId).Distinct().ToArrayAsync(cancellationToken);

                var productQuery = db.Products
                    .Include(p => p.Images)
                    .Where(p => productIds.Contains(p.Id))
                    .AsQueryable();


                if (request.Brands != null && request.Brands.Length > 0)
                {
                    productQuery = productQuery.Where(q => request.Brands.Contains(q.BrandId));
                }

                if (request.Min > 0 && request.Min <= request.Max)
                {
                    productQuery = productQuery.Where(q => q.Price >= request.Min && q.Price <= request.Max);
                }

                productQuery = productQuery
                    .OrderByDescending(q => q.Id);

                var pagedModel = new PagedViewModel<Product>(productQuery, request);

                return pagedModel;

            }
        }
    }
}
