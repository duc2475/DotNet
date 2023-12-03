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
    public class PromotionsController : Controller
    {
        private readonly ecommerceContext _context;

        public PromotionsController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/Promotions
        public async Task<IActionResult> Index()
        {
              return _context.TblPromotions != null ? 
                          View(await _context.TblPromotions.ToListAsync()) :
                          Problem("Entity set 'ecommerceContext.TblPromotions'  is null.");
        }

        // GET: admin/Promotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblPromotions == null)
            {
                return NotFound();
            }

            var tblPromotion = await _context.TblPromotions
                .FirstOrDefaultAsync(m => m.PromoId == id);
            if (tblPromotion == null)
            {
                return NotFound();
            }

            return View(tblPromotion);
        }

        // GET: admin/Promotions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Promotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PromoId,PromoName,PromoDiscount,PromoSdate,PromoEdate")] TblPromotion tblPromotion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblPromotion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblPromotion);
        }

        // GET: admin/Promotions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblPromotions == null)
            {
                return NotFound();
            }

            var tblPromotion = await _context.TblPromotions.FindAsync(id);
            if (tblPromotion == null)
            {
                return NotFound();
            }
            return View(tblPromotion);
        }

        // POST: admin/Promotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PromoId,PromoName,PromoDiscount,PromoSdate,PromoEdate")] TblPromotion tblPromotion)
        {
            if (id != tblPromotion.PromoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPromotionExists(tblPromotion.PromoId))
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
            return View(tblPromotion);
        }

        // GET: admin/Promotions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPromotions == null)
            {
                return NotFound();
            }

            var tblPromotion = await _context.TblPromotions
                .FirstOrDefaultAsync(m => m.PromoId == id);
            if (tblPromotion == null)
            {
                return NotFound();
            }

            return View(tblPromotion);
        }

        // POST: admin/Promotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblPromotions == null)
            {
                return Problem("Entity set 'ecommerceContext.TblPromotions'  is null.");
            }
            var tblPromotion = await _context.TblPromotions.FindAsync(id);
            if (tblPromotion != null)
            {
                _context.TblPromotions.Remove(tblPromotion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPromotionExists(int id)
        {
          return (_context.TblPromotions?.Any(e => e.PromoId == id)).GetValueOrDefault();
        }
    }
}
