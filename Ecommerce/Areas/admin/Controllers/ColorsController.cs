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
    public class ColorsController : Controller
    {
        private readonly ecommerceContext _context;

        public ColorsController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/Colors
        public async Task<IActionResult> Index()
        {
              return _context.TblColors != null ? 
                          View(await _context.TblColors.ToListAsync()) :
                          Problem("Entity set 'ecommerceContext.TblColors'  is null.");
        }

        // GET: admin/Colors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblColors == null)
            {
                return NotFound();
            }

            var tblColor = await _context.TblColors
                .FirstOrDefaultAsync(m => m.ColorId == id);
            if (tblColor == null)
            {
                return NotFound();
            }

            return View(tblColor);
        }

        // GET: admin/Colors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Colors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ColorId,ColorName")] TblColor tblColor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblColor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblColor);
        }

        // GET: admin/Colors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblColors == null)
            {
                return NotFound();
            }

            var tblColor = await _context.TblColors.FindAsync(id);
            if (tblColor == null)
            {
                return NotFound();
            }
            return View(tblColor);
        }

        // POST: admin/Colors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ColorId,ColorName")] TblColor tblColor)
        {
            if (id != tblColor.ColorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblColor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblColorExists(tblColor.ColorId))
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
            return View(tblColor);
        }

        // GET: admin/Colors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblColors == null)
            {
                return NotFound();
            }

            var tblColor = await _context.TblColors
                .FirstOrDefaultAsync(m => m.ColorId == id);
            if (tblColor == null)
            {
                return NotFound();
            }

            return View(tblColor);
        }

        // POST: admin/Colors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblColors == null)
            {
                return Problem("Entity set 'ecommerceContext.TblColors'  is null.");
            }
            var tblColor = await _context.TblColors.FindAsync(id);
            if (tblColor != null)
            {
                _context.TblColors.Remove(tblColor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblColorExists(int id)
        {
          return (_context.TblColors?.Any(e => e.ColorId == id)).GetValueOrDefault();
        }
    }
}
