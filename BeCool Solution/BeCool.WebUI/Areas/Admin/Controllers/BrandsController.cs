using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandsController : Controller
    {
        private readonly BeCoolDbContext db;

        public BrandsController(BeCoolDbContext db)
        {
            this.db = db;
        }

        // GET: Admin/Brands
        [Authorize(Policy = "admin.brands.index")]
        public async Task<IActionResult> Index()
        {
            var data = await db.Brands
                .Where(m => m.DeletedDate == null)
                .ToListAsync();
            return View(data);
        }

        // GET: Admin/Brands/Details/5
        [Authorize(Policy = "admin.brands.details")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await db.Brands
                .FirstOrDefaultAsync(m => m.Id == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/Brands/Create
        [Authorize(Policy = "admin.brands.create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brands/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Policy = "admin.brands.create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                db.Brands.Add(brand);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }

        // GET: Admin/Brands/Edit/5
        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await db.Brands.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        // POST: Admin/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.edit")]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedDate,DeletedDate")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(brand);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(brand);
        }


        // POST: Admin/Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.brands.remove")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brand = await db.Brands.FindAsync(id);

            if (brand == null)
            {
                return Json( new
                {
                    error = true,
                    message = "Melumat movcud deyil"
                });
            }

            db.Brands.Remove(brand);
            await db.SaveChangesAsync();


            return Json(
                new
                {
                    error = false,
                    message = "Melumat silindi"
                });
        }

        private bool BrandExists(int id)
        {
            return db.Brands.Any(e => e.Id == id);
        }
    }
}
