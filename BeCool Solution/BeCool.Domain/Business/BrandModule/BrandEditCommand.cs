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
    public class BrandEditCommand : IRequest<Brand>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class BrandEditCommandHandler : IRequestHandler<BrandEditCommand, Brand>
        {
            private readonly BeCoolDbContext db;

            public BrandEditCommandHandler(BeCoolDbContext db)
            {
                this.db = db;
            }
            public async Task<Brand> Handle(BrandEditCommand request, CancellationToken cancellationToken)
            {
                var data = await db.Brands
                    .FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null, cancellationToken);

                if (data == null)
                {
                    return null;
                }

                data.Name = request.Name;
                await db.SaveChangesAsync(cancellationToken);


                return data;
            }
        }
    }
}
