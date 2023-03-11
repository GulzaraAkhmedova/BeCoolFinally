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

namespace BeCool.Domain.Business.BrandModule
{
    public class BrandRemoveCommand : IRequest<Brand>
    {
        public int Id { get; set; }

        public class BrandRemoveCommandHandler : IRequestHandler<BrandRemoveCommand, Brand>
        {
            private readonly BeCoolDbContext db;

            public BrandRemoveCommandHandler(BeCoolDbContext db)
            {
                this.db = db;
            }
            public async Task<Brand> Handle(BrandRemoveCommand request, CancellationToken cancellationToken)
            {
                var data = await db.Brands
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.DeletedDate = DateTime.UtcNow.AddHours(4);
                await db.SaveChangesAsync(cancellationToken);


                return data;
            }
        }
    }
}
