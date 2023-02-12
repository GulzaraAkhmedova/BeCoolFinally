using MediatR;
using Microsoft.EntityFrameworkCore;
using BeCool.Application.Infrastructure;
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
    public class BlogPostGetAllQuery : PaginateModel, IRequest<PagedViewModel<BlogPost>>
    {
        public class BlogPostGetAllQueryHandler : IRequestHandler<BlogPostGetAllQuery, PagedViewModel<BlogPost>>
        {
            private readonly BeCoolDbContext db;

            public BlogPostGetAllQueryHandler(BeCoolDbContext db)
            {
                this.db = db;
            }
            public async Task<PagedViewModel<BlogPost>> Handle(BlogPostGetAllQuery request, CancellationToken cancellationToken)
            {
                if (request.PageSize < 6)
                {
                    request.PageSize = 6;
                }

                var query = db.BlogPosts
                        .Where(bp => bp.DeletedDate == null && bp.PublishedDate != null)
                        .OrderByDescending(bp => bp.PublishedDate)
                        .AsQueryable();
                
                        //.Skip(skipSize)
                        //.Take(request.PageSize)
                var pageModel = new PagedViewModel<BlogPost>(query,request);



                return pageModel;
            }
        }
    }
}
