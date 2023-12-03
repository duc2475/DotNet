using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductPicsController : Controller
    {
        private readonly ecommerceContext _context;

        public ProductPicsController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/ProductPics
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.TblProductPics.Include(t => t.Product);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: admin/ProductPics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblProductPics == null)
            {
                return NotFound();
            }

            var tblProductPic = await _context.TblProductPics
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.PicId == id);
            if (tblProductPic == null)
            {
                return NotFound();
            }

            return View(tblProductPic);
        }

        // GET: admin/ProductPics/Create
        public IActionResult Create()
        {
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId");
            return View();
        }

        // POST: admin/ProductPics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PicId,PicName,ProductId")] TblProductPic tblProductPic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblProductPic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId", tblProductPic.ProductId);
            return View(tblProductPic);
        }

        // GET: admin/ProductPics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblProductPics == null)
            {
                return NotFound();
            }

            var tblProductPic = await _context.TblProductPics.FindAsync(id);
            if (tblProductPic == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId", tblProductPic.ProductId);
            return View(tblProductPic);
        }

        // POST: admin/ProductPics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PicId,PicName,ProductId")] TblProductPic tblProductPic)
        {
            if (id != tblProductPic.PicId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProductPic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductPicExists(tblProductPic.PicId))
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
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId", tblProductPic.ProductId);
            return View(tblProductPic);
        }

        // GET: admin/ProductPics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblProductPics == null)
            {
                return NotFound();
            }

            var tblProductPic = await _context.TblProductPics
                .Include(t => t.Product)
                .FirstOrDefaultAsync(m => m.PicId == id);
            if (tblProductPic == null)
            {
                return NotFound();
            }

            return View(tblProductPic);
        }

        // POST: admin/ProductPics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblProductPics == null)
            {
                return Problem("Entity set 'ecommerceContext.TblProductPics'  is null.");
            }
            var tblProductPic = await _context.TblProductPics.FindAsync(id);
            if (tblProductPic != null)
            {
                _context.TblProductPics.Remove(tblProductPic);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProductPicExists(int id)
        {
          return (_context.TblProductPics?.Any(e => e.PicId == id)).GetValueOrDefault();
        }
    }
}
