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
    public class MidCategoriesController : Controller
    {
        private readonly ecommerceContext _context;

        public MidCategoriesController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/MidCategories
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.TblMidCategories.Include(t => t.Tcat);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: admin/MidCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblMidCategories == null)
            {
                return NotFound();
            }

            var tblMidCategory = await _context.TblMidCategories
                .Include(t => t.Tcat)
                .FirstOrDefaultAsync(m => m.McatId == id);
            if (tblMidCategory == null)
            {
                return NotFound();
            }

            return View(tblMidCategory);
        }

        // GET: admin/MidCategories/Create
        public IActionResult Create()
        {
            ViewData["TcatId"] = new SelectList(_context.TblTopCategories, "TcatId", "TcatName");
            return View();
        }

        // POST: admin/MidCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("McatId,McatName,TcatId")] TblMidCategory tblMidCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblMidCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TcatId"] = new SelectList(_context.TblTopCategories, "TcatId", "TcatName", tblMidCategory.TcatId);
            return View(tblMidCategory);
        }

        // GET: admin/MidCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblMidCategories == null)
            {
                return NotFound();
            }

            var tblMidCategory = await _context.TblMidCategories.FindAsync(id);
            if (tblMidCategory == null)
            {
                return NotFound();
            }
            ViewData["TcatId"] = new SelectList(_context.TblTopCategories, "TcatId", "TcatName", tblMidCategory.TcatId.ToString());
            return View(tblMidCategory);
        }

        // POST: admin/MidCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("McatId,McatName,TcatId")] TblMidCategory tblMidCategory)
        {
            if (id != tblMidCategory.McatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblMidCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblMidCategoryExists(tblMidCategory.McatId))
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
            ViewData["TcatId"] = new SelectList(_context.TblTopCategories, "TcatId", "TcatId", tblMidCategory.TcatId);
            return View(tblMidCategory);
        }

        // GET: admin/MidCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblMidCategories == null)
            {
                return NotFound();
            }

            var tblMidCategory = await _context.TblMidCategories
                .Include(t => t.Tcat)
                .FirstOrDefaultAsync(m => m.McatId == id);
            if (tblMidCategory == null)
            {
                return NotFound();
            }

            return View(tblMidCategory);
        }

        // POST: admin/MidCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblMidCategories == null)
            {
                return Problem("Entity set 'ecommerceContext.TblMidCategories'  is null.");
            }
            var tblMidCategory = await _context.TblMidCategories.FindAsync(id);
            if (tblMidCategory != null)
            {
                _context.TblMidCategories.Remove(tblMidCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblMidCategoryExists(int id)
        {
          return (_context.TblMidCategories?.Any(e => e.McatId == id)).GetValueOrDefault();
        }
    }
}
