using MediatR;
using Microsoft.Extensions.Hosting;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using BeCool.Application.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using BeCool.Domain.AppCode.Extensions;

namespace BeCool.Domain.Business.ProductModule
{
    public class ProductEditCommand : IRequest<Product>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string StockKeepingUnit { get; set; }
        public decimal Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public ImageItem[] Images { get; set; }

        public class ProductEditCommandHandler : IRequestHandler<ProductEditCommand, Product>
        {
            private readonly BeCoolDbContext db;
            private readonly IHostEnvironment env;
            private readonly IActionContextAccessor ctx;

            public ProductEditCommandHandler(BeCoolDbContext db, IHostEnvironment env, IActionContextAccessor ctx)
            {
                this.db = db;
                this.env = env;
                this.ctx = ctx;
            }
            public async Task<Product> Handle(ProductEditCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var model = await db.Products
                        .Include(p => p.Images.Where(i => i.DeletedUserId == null))
                        .FirstOrDefaultAsync(p =>
                        p.Id == request.Id
                        && p.DeletedUserId == null
                        ,cancellationToken);

                    if (model == null)
                    {
                        return null;
                    }


                    model.Name = request.Name;
                    model.StockKeepingUnit = request.StockKeepingUnit;
                    model.Price = request.Price;
                    model.ShortDescription = request.ShortDescription;
                    model.Description = request.Description;
                    model.BrandId = request.BrandId;
                    model.CategoryId = request.CategoryId;



                    if (request.Images != null && request.Images.Count() > 0)
                    {

                        #region Teze fayllar var

                        foreach (var imageItem in request.Images.Where(i => i.File != null && i.Id == null))
                        {
                            var image = new ProductImage();
                            image.IsMain = imageItem.IsMain;
                            image.ProductId = model.Id;

                            string extension = Path.GetExtension(imageItem.File.FileName);

                            image.Name = $"product-{Guid.NewGuid().ToString().ToLower()}{extension}";

                            string fullName = env.GetImagePhysicalPath(image.Name);

                            using (var fs = new FileStream(fullName, FileMode.Create, FileAccess.Write))
                            {
                                await imageItem.File.CopyToAsync(fs, cancellationToken);
                            }
                            model.Images.Add(image);

                        }

                        #endregion


                        #region Movcud sekillerden silibse

                        foreach (var item in request.Images.Where(i => i.Id > 0 && i.TempPath == null))
                        {
                            var productImage = await db.ProductImages.FirstOrDefaultAsync(pi => pi.Id == item.Id && pi.ProductId == model.Id && pi.DeletedUserId == null);

                            if (productImage != null)
                            {
                                productImage.IsMain = false;
                                productImage.DeletedDate = DateTime.UtcNow.AddHours(4);
                                productImage.DeletedUserId = ctx.GetCurrentUserId();
                            }


                        }

                        #endregion

                        #region Movcud sekilde deyisiklik var

                        foreach (var item in model.Images)
                        {
                            var fromForm = request.Images.FirstOrDefault(i => i.Id == item.Id);

                            if (fromForm != null)
                            {
                                item.IsMain = fromForm.IsMain;
                            }

                        }

                        #endregion
                    }

                    await db.SaveChangesAsync(cancellationToken);
                    return model;
                }
                catch (System.Exception)
                {

                    return null;
                }
            }
        }
    }
}
