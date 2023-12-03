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
    public class EndCategoriesController : Controller
    {
        private readonly ecommerceContext _context;

        public EndCategoriesController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/EndCategories
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.TblEndCategories.Include(t => t.Mcat);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: admin/EndCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblEndCategories == null)
            {
                return NotFound();
            }

            var tblEndCategory = await _context.TblEndCategories
                .Include(t => t.Mcat)
                .FirstOrDefaultAsync(m => m.EcatId == id);
            if (tblEndCategory == null)
            {
                return NotFound();
            }

            return View(tblEndCategory);
        }

        // GET: admin/EndCategories/Create
        public IActionResult Create()
        {
            ViewBag.McatId = new SelectList(_context.TblMidCategories, "McatId", "McatName");
            return View();
        }

        // POST: admin/EndCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EcatId,EcatName,McatId")] TblEndCategory tblEndCategory)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblEndCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.McatId = new SelectList(_context.TblMidCategories, "McatId", "McatName", tblEndCategory.McatId.ToString());
            return View(tblEndCategory);
        }

        // GET: admin/EndCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblEndCategories == null)
            {
                return NotFound();
            }

            var tblEndCategory = await _context.TblEndCategories.FindAsync(id);
            if (tblEndCategory == null)
            {
                return NotFound();
            }
            ViewData["McatId"] = new SelectList(_context.TblMidCategories, "McatId", "McatName", tblEndCategory.McatId.ToString());
            return View(tblEndCategory);
        }

        // POST: admin/EndCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EcatId,EcatName,McatId")] TblEndCategory tblEndCategory)
        {
            if (id != tblEndCategory.EcatId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblEndCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblEndCategoryExists(tblEndCategory.EcatId))
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
            ViewData["McatId"] = new SelectList(_context.TblMidCategories, "McatId", "McatId", tblEndCategory.McatId);
            return View(tblEndCategory);
        }

        // GET: admin/EndCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblEndCategories == null)
            {
                return NotFound();
            }

            var tblEndCategory = await _context.TblEndCategories
                .Include(t => t.Mcat)
                .FirstOrDefaultAsync(m => m.EcatId == id);
            if (tblEndCategory == null)
            {
                return NotFound();
            }

            return View(tblEndCategory);
        }

        // POST: admin/EndCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblEndCategories == null)
            {
                return Problem("Entity set 'ecommerceContext.TblEndCategories'  is null.");
            }
            var tblEndCategory = await _context.TblEndCategories.FindAsync(id);
            if (tblEndCategory != null)
            {
                _context.TblEndCategories.Remove(tblEndCategory);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblEndCategoryExists(int id)
        {
          return (_context.TblEndCategories?.Any(e => e.EcatId == id)).GetValueOrDefault();
        }
    }
}
