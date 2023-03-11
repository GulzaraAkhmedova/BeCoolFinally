using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SizesController : Controller
    {
        private readonly BeCoolDbContext _context;

        public SizesController(BeCoolDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Sizes
        public async Task<IActionResult> Index()
        {
            return View(await _context.ProductSizes.ToListAsync());
        }

        // GET: Admin/Sizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // GET: Admin/Sizes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Sizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreatedDate,DeletedDate")] ProductSize productSize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productSize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(productSize);
        }

        // GET: Admin/Sizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes.FindAsync(id);
            if (productSize == null)
            {
                return NotFound();
            }
            return View(productSize);
        }

        // POST: Admin/Sizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Id,CreatedDate,DeletedDate")] ProductSize productSize)
        {
            if (id != productSize.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productSize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductSizeExists(productSize.Id))
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
            return View(productSize);
        }

        // GET: Admin/Sizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productSize = await _context.ProductSizes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (productSize == null)
            {
                return NotFound();
            }

            return View(productSize);
        }

        // POST: Admin/Sizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productSize = await _context.ProductSizes.FindAsync(id);
            _context.ProductSizes.Remove(productSize);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductSizeExists(int id)
        {
            return _context.ProductSizes.Any(e => e.Id == id);
        }
    }
}
