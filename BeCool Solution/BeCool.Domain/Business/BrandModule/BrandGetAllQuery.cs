using MediatR;
using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BeCool.Domain.Models.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace BeCool.Domain.Business.BrandModule
{
    public class BrandGetAllQuery : IRequest<List<Brand>>
    {
        public class BrandGetAllQueryHandler : IRequestHandler<BrandGetAllQuery, List<Brand>>
        {
            private readonly BeCoolDbContext db;

            public BrandGetAllQueryHandler(BeCoolDbContext db)
            {
                this.db = db;
            }

            public async Task<List<Brand>> Handle(BrandGetAllQuery request, CancellationToken cancellationToken)
            {
                var data = await db.Brands.Where(m => m.DeletedDate == null)
               .ToListAsync(cancellationToken);

                return data;
            }
        }
    }
}
