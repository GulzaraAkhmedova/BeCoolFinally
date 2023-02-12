using MediatR;
using Microsoft.EntityFrameworkCore;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeCool.Domain.Business.BlogPostModule
{
    public class BlogPostSingleQuery : IRequest<BlogPost>
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public class BlogPostSingleQueryQueryHandler : IRequestHandler<BlogPostSingleQuery, BlogPost>
        {
            private readonly BeCoolDbContext db;

            public BlogPostSingleQueryQueryHandler(BeCoolDbContext db)
            {
                this.db = db;
            }
            public async Task<BlogPost> Handle(BlogPostSingleQuery request, CancellationToken cancellationToken)
            {
                var query = db.BlogPosts
                .Include(bp => bp.Category)
                .Include(bp => bp.Comments)
                .Include(bp => bp.TagCloud)
                .ThenInclude(bp => bp.Tag)
                .AsQueryable();

                if (string.IsNullOrWhiteSpace(request.Slug))
                {
                    return await query.FirstOrDefaultAsync(m => m.Id == request.Id && m.DeletedDate == null);
                }

                return await query.FirstOrDefaultAsync(m => m.Slug.Equals(request.Slug) && m.DeletedDate == null);
            }
        }
    }
}
