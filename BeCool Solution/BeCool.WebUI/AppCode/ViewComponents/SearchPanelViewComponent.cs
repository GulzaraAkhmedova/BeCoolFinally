using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BeCool.WebUI.AppCode.ViewComponents
{
    public class SearchPanelViewComponent : ViewComponent
    {
        private readonly BeCoolDbContext db;

        public SearchPanelViewComponent(BeCoolDbContext db)
        {
            this.db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var vm = new SearchPanelViewModel();

            vm.Materials = db.ProductCatalog
                .Include(pv => pv.ProductMaterial)
                .Select(pv => pv.ProductMaterial)
                .Distinct()
                .ToArray();


            vm.Sizes= db.ProductCatalog
                .Include(pa => pa.ProductSize)
                .Select(pa => pa.ProductSize)
                .Distinct()
                .ToArray();

            vm.Types = db.ProductCatalog
                .Include(pt => pt.ProductType)
                .Select(pt => pt.ProductType)
                .Distinct()
                .ToArray();

            vm.Brands = db.ProductCatalog
                .Include(pb => pb.Product)
                .ThenInclude(pb => pb.Brand)
                .Select(pb => pb.Product.Brand)
                .Distinct()
                .ToArray();

            vm.Categories = db.ProductCatalog
                .Include(c => c.Product)
                .ThenInclude(c => c.Category)
                .Select(c => c.Product.Category)
                .Distinct()
                .ToArray();




            var priceRange = db.ProductCatalog
                .Include(pc => pc.Product)
                .Select(pc => pc.Product.Price)
                .ToArray();

            vm.Min = (int)Math.Floor(priceRange.Min());
            vm.Max = (int)Math.Floor(priceRange.Max());

            return View(vm);
        }
    }
}
