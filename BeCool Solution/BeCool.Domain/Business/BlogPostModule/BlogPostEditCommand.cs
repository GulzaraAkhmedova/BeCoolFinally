using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using BeCool.Application.Extensions;
using BeCool.Application.Infrastructure;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BeCool.Domain.Business.BlogPostModule
{
    public class BlogPostEditCommand : IRequest<JsonResponse>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string ImagePath { get; set; }
        public int CategoryId { get; set; }
        public IFormFile Image { get; set; }
        public int[] TagIds { get; set; }
        public class BlogPostEditCommandHandler : IRequestHandler<BlogPostEditCommand, JsonResponse>
        {
            private readonly BeCoolDbContext db;
            private readonly IHostEnvironment env;

            public BlogPostEditCommandHandler(BeCoolDbContext db, IHostEnvironment env)
            {
                this.db = db;
                this.env = env;
            }
            public async Task<JsonResponse> Handle(BlogPostEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.BlogPosts
                    .Include(bp => bp.TagCloud)
                    .FirstOrDefaultAsync(bp => bp.Id == request.Id && bp.DeletedDate == null);

                if (entity == null)
                {
                    return null;
                }

                entity.Title = request.Title;
                entity.Body = request.Body;
                entity.CategoryId = request.CategoryId;

                if (request.Image == null)
                    goto end;


                string extension = Path.GetExtension(request.Image.FileName);

                request.ImagePath = $"blogpost-{Guid.NewGuid().ToString().ToLower()}{extension}";

                string fullPath = env.GetImagePhysicalPath(request.ImagePath);

                using (var fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    request.Image.CopyTo(fs);
                }

                //var oldPath = env.GetImagePhysicalPath(entity.ImagePath);

                //if (System.IO.File.Exists(oldPath))
                //{
                //    System.IO.File.Delete(oldPath);
                //}

                env.ArchiveImage(entity.ImagePath);

                entity.ImagePath = request.ImagePath;

            end:
                if (string.IsNullOrWhiteSpace(entity.Slug))
                {
                    entity.Slug = request.Title.ToSlug();
                }

                if (request.TagIds == null && entity.TagCloud.Any())
                {
                    foreach (var tagItem in entity.TagCloud)
                    {
                        db.BlogPostTagCloud.Remove(tagItem);
                    }
                }
                else if (request.TagIds != null)
                {

                    #region database-de evvel olan indi olmayan taglar silinmesini istediklerimiz
                    //1,2,3 => 1,3
                    var exceptedIds = db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()
                        .Except(request.TagIds).ToArray();

                    if (exceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in exceptedIds)
                        {
                            var tagItem = db.BlogPostTagCloud.ToList().FirstOrDefault(tc => tc.TagId == exceptedId && tc.BlogPostId == entity.Id);

                            if (tagItem != null)
                            {
                                db.BlogPostTagCloud.Remove(tagItem);
                            }
                        }
                    }
                    #endregion

                    #region evvel databasede olmayan ama indi elave olunmasini istediklerimiz
                    var NewExceptedIds = request.TagIds.Except(db.BlogPostTagCloud.Where(tc => tc.BlogPostId == entity.Id).Select(tc => tc.TagId).ToList()).ToArray();

                    if (NewExceptedIds.Length > 0)
                    {
                        foreach (var exceptedId in NewExceptedIds)
                        {
                            var tagItem = new BlogPostTagItem();
                            tagItem.TagId = exceptedId;
                            tagItem.BlogPostId = entity.Id;
                            await db.BlogPostTagCloud.AddAsync(tagItem);
                        }
                    }
                    #endregion
                }


                await db.SaveChangesAsync(cancellationToken);
                
                return new JsonResponse
                {
                    Error = false,
                    Message = "Success"
                };
            }
        }
    }
}
