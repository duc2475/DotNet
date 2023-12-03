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
    public class TopCategoriesController : Controller
    {
        private readonly ecommerceContext _context;

        public TopCategoriesController(ecommerceContext context)
        { 
            _context = context;
        }

        // GET: admin/TopCategories
        public async Task<IActionResult> Index()
        {
              return _context.TblTopCategories != null ? 
                          View(await _context.TblTopCategories.ToListAsync()) :
                          Problem("Entity set 'ecommerceContext.TblTopCategories'  is null.");
        }

        // GET: admin/TopCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblTopCategories == null)
            {
                return NotFound();
            }

            var tblTopCategory = await _context.TblTopCategories
                .FirstOrDefaultAsync(m => m.TcatId == id);
            if (tblTopCategory == null)
            {
                return NotFound();
            }

            return View(tblTopCategory);
        }

        // GET: admin/TopCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/TopCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TcatId,TcatName,ShowOnMenu")] TblTopCategory tblTopCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblTopCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblTopCategory);
        }

        // GET: admin/TopCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblTopCategories == null)
            {
                return NotFound();
            }

            var tblTopCategory = await _context.TblTopCategories.FindAsync(id);
            if (tblTopCategory == null)
            {
                return NotFound();
            }
            return View(tblTopCategory);
        }

        // POST: admin/TopCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TcatId,TcatName,ShowOnMenu")] TblTopCategory tblTopCategory)
        {
            if (id != tblTopCategory.TcatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblTopCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblTopCategoryExists(tblTopCategory.TcatId))
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
            return View(tblTopCategory);
        }

        // GET: admin/TopCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblTopCategories == null)
            {
                return NotFound();
            }

            var tblTopCategory = await _context.TblTopCategories
                .FirstOrDefaultAsync(m => m.TcatId == id);
            if (tblTopCategory == null)
            {
                return NotFound();
            }

            return View(tblTopCategory);
        }

        // POST: admin/TopCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblTopCategories == null)
            {
                return Problem("Entity set 'ecommerceContext.TblTopCategories'  is null.");
            }
            var tblTopCategory = await _context.TblTopCategories.FindAsync(id);
            if (tblTopCategory != null)
            {
                _context.TblTopCategories.Remove(tblTopCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblTopCategoryExists(int id)
        {
          return (_context.TblTopCategories?.Any(e => e.TcatId == id)).GetValueOrDefault();
        }
    }
}
