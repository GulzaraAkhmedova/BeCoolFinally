using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeCool.Domain.AppCode.Extensions;
using BeCool.Domain.Business.ProductModule;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        private readonly BeCoolDbContext db;
        private readonly IMediator mediator;

        public ProductsController(BeCoolDbContext db,IMediator mediator)
        {
            this.db = db;
            this.mediator = mediator;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(ProductsPagedQuery query)
        {
            var response = await mediator.Send(query);
            return View(response);
        }

        // GET: Admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/Products/Create
        public IActionResult Create()
        {
            ViewBag.BrandId = new SelectList(db.Brands, "Id", "Name");
            ViewBag.CategoryId = new SelectList(db.Categories, "Id", "Name");
            return View();
        }

        // POST: Admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateCommand command)
        {
            
            var response = await mediator.Send(command);

            if (response == null)
            {
                ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", command.BrandId);
                ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
                return View(command);
            }
            return RedirectToAction(nameof(Index));

        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(ProductSingleQuery query)
        {
            var product = await mediator.Send(query);
            if (product == null)
            {
                return NotFound();
            }

            var command = new ProductEditCommand();
            command.Name = product.Name;
            command.Price = product.Price;
            command.ShortDescription = product.ShortDescription;
            command.Description = product.Description;
            command.BrandId = product.BrandId;
            command.CategoryId = product.CategoryId;
            command.StockKeepingUnit = product.StockKeepingUnit;

            command.Images = product.Images.Select(x => new ImageItem
            {
                Id = x.Id,
                TempPath = x.Name,
                IsMain = x.IsMain
            }).ToArray();



            ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", product.BrandId);
            ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", product.CategoryId);
            return View(command);
        }

        // POST: Admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductEditCommand command)
        {
            if (id != command.Id)
            {
                return NotFound();
            }

            var response = await mediator.Send(command);

            if (response == null)
            {
                ViewData["BrandId"] = new SelectList(db.Brands, "Id", "Name", command.BrandId);
                ViewData["CategoryId"] = new SelectList(db.Categories, "Id", "Name", command.CategoryId);
                return View(command);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.products.remove")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await db.Products.FirstOrDefaultAsync(p => p.Id == id && p.DeletedDate == null);
            //Product product = null;
            if (product == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Melumat movcud deyil"
                });
            }

            product.DeletedDate = DateTime.UtcNow.AddHours(4);
            product.DeletedUserId = User.GetCurrentUserId();
            await db.SaveChangesAsync();

            var response = await mediator.Send(new ProductsPagedQuery());

            return PartialView("_ListBody", response);

        }

        private bool ProductExists(int id)
        {
            return db.Products.Any(e => e.Id == id);
        }
    }
}
