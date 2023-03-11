using BeCool.Domain.Models.DataContexts;
using BeCool.Domain.Models.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace BeCool.WebUI.Areas.Admin.Controllers
{
    public class ProductCatalogController :Controller
    {
        private readonly BeCoolDbContext _context;

        public ProductCatalogController(BeCoolDbContext context)
        {
            _context = context;
        }

        // GET: Admin/ProductCatalog
        [Authorize(Policy = "admin.productcatalog.index")]
        public async Task<IActionResult> Index()
        {
            var becoolDbContext = _context.ProductCatalog.Include(p => p.CreatedByUser).Include(p => p.DeletedUser).Include(p => p.Product).Include(p => p.ProductSize).Include(p => p.ProductType).Include(p => p.ProductMaterial);
            return View(await becoolDbContext.ToListAsync());
        }

        // GET: Admin/ProductCatalog/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalogItem = await _context.ProductCatalog
                .Include(p => p.CreatedByUser)
                .Include(p => p.DeletedUser)
                .Include(p => p.Product)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductType)
                .Include(p => p.ProductMaterial)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productCatalogItem == null)
            {
                return NotFound();
            }

            return View(productCatalogItem);
        }

        // GET: Admin/ProductCatalog/Create
        public IActionResult Create()
        {
            ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["DeletedUserId"] = new SelectList(_context.Users, "Id", "Name");
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name");
            ViewData["ProductSizeId"] = new SelectList(_context.ProductSizes, "Id", "Name");
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name");
            ViewData["ProductMaterialId"] = new SelectList(_context.ProductMaterials, "Id", "Name");
            return View();
        }

        // POST: Admin/ProductCatalog/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductViscosityId,ProductLiterId,ProductApiId,ProductTypeId,Id,CreatedByUserId,CreatedDate,DeletedDate,DeletedUserId")] ProductCatalogItem productCatalogItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productCatalogItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DeletedUserId"] = new SelectList(_context.Users, "Id", "Name", productCatalogItem.DeletedUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Id", productCatalogItem.ProductId);
            ViewData["ProductSizeId"] = new SelectList(_context.ProductSizes, "Id", "Id", productCatalogItem.ProductSizeId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Id", productCatalogItem.ProductTypeId);
            ViewData["ProductMaterialId"] = new SelectList(_context.ProductMaterials, "Id", "Id", productCatalogItem.ProductMaterialId);
            return View(productCatalogItem);
        }

        // GET: Admin/ProductCatalog/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalogItem = await _context.ProductCatalog.FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCatalogItem == null)
            {
                return NotFound();
            }
            //ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Name", productCatalogItem.CreatedByUserId);
            //ViewData["DeletedUserId"] = new SelectList(_context.Users, "Id", "Name", productCatalogItem.DeletedUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productCatalogItem.ProductId);
            ViewData["ProductSizeId"] = new SelectList(_context.ProductSizes, "Id", "Name", productCatalogItem.ProductSizeId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", productCatalogItem.ProductTypeId);
            ViewData["ProductMaterialId"] = new SelectList(_context.ProductMaterials, "Id", "Name", productCatalogItem.ProductMaterialId);
            return View(productCatalogItem);
        }

        // POST: Admin/ProductCatalog/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductViscosityId,ProductLiterId,ProductApiId,ProductTypeId,Id,CreatedByUserId,CreatedDate,DeletedDate,DeletedUserId")] ProductCatalogItem productCatalogItem)
        {
            if (id != productCatalogItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productCatalogItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductCatalogItemExists(productCatalogItem.Id))
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
            //ViewData["CreatedByUserId"] = new SelectList(_context.Users, "Id", "Name", productCatalogItem.CreatedByUserId);
            //ViewData["DeletedUserId"] = new SelectList(_context.Users, "Id", "Name", productCatalogItem.DeletedUserId);
            ViewData["ProductId"] = new SelectList(_context.Products, "Id", "Name", productCatalogItem.ProductId);
            ViewData["ProductSizeId"] = new SelectList(_context.ProductSizes, "Id", "Name", productCatalogItem.ProductSizeId);
            ViewData["ProductTypeId"] = new SelectList(_context.ProductTypes, "Id", "Name", productCatalogItem.ProductTypeId);
            ViewData["ProductMaterialId"] = new SelectList(_context.ProductMaterials, "Id", "Name", productCatalogItem.ProductMaterialId);
            return View(productCatalogItem);
        }

        // GET: Admin/ProductCatalog/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productCatalogItem = await _context.ProductCatalog
                .Include(p => p.CreatedByUser)
                .Include(p => p.DeletedUser)
                .Include(p => p.Product)
                .Include(p => p.ProductSize)
                .Include(p => p.ProductType)
                .Include(p => p.ProductMaterial)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (productCatalogItem == null)
            {
                return NotFound();
            }

            return View(productCatalogItem);
        }

        // POST: Admin/ProductCatalog/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var productCatalogItem = await _context.ProductCatalog.FindAsync(id);
        //    _context.ProductCatalog.Remove(productCatalogItem);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "admin.productcatalog.remove")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var catalog = await _context.ProductCatalog.FirstOrDefaultAsync(pc => pc.Id == id && pc.DeletedDate == null);

            if (catalog == null)
            {
                return Json(new
                {
                    error = true,
                    message = "Melumat movcud deyil"
                });
            }

            _context.ProductCatalog.Remove(catalog);
            await _context.SaveChangesAsync();


            return Json(
                new
                {
                    error = false,
                    message = "Melumat silindi"
                });
        }

        private bool ProductCatalogItemExists(int id)
        {
            return _context.ProductCatalog.Any(e => e.ProductId == id);
        }
    }
}

