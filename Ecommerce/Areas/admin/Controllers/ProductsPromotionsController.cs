using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Http.Extensions;

namespace Ecommerce.Areas.admin.Controllers
{
    
    
    [Area("admin")]
    public class ProductsPromotionsController : Controller
    {
        private readonly ecommerceContext _context;

        public ProductsPromotionsController(ecommerceContext context)
        {
            _context = context;
        }



        // GET: admin/ProductsPromotions
        public IActionResult Index(int? page, int id)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsProduct = _context.TblProductsPromotions
                .AsNoTracking()
                .Include(x => x.Product)
                .Include(x => x.Promo)
                .OrderByDescending(x => x.PpId)
                .Where(x => x.PromoId == id);
            PagedList<TblProductsPromotion> models = new PagedList<TblProductsPromotion>(lsProduct, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.id = id;
            ViewBag.PromoName = _context.TblPromotions.FirstOrDefault(e => e.PromoId == id);
            return View(models);
        }

        // GET: admin/ProductsPromotions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblProductsPromotions == null)
            {
                return NotFound();
            }

            var tblProductsPromotion = await _context.TblProductsPromotions
                .Include(t => t.Product)
                .Include(t => t.Promo)
                .FirstOrDefaultAsync(m => m.PpId == id);
            if (tblProductsPromotion == null)
            {
                return NotFound();
            }

            return View(tblProductsPromotion);
        }

        // GET: admin/ProductsPromotions/Create
        public IActionResult Create(int? page, int promoid)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsProduct = _context.TblProducts
                .AsNoTracking()
                .OrderByDescending(x => x.ProductId);
            PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Promo = _context.TblPromotions.FirstOrDefault(e => e.PromoId == promoid);
            return View(models);
        }

        // POST: admin/ProductsPromotions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int? page,int id, int pp)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsProduct = _context.TblProducts
                .AsNoTracking()
                .OrderByDescending(x => x.ProductId);
            PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.Promo = _context.TblPromotions.FirstOrDefault(e => e.PromoId == pp);
            var PP = _context.TblProductsPromotions
                .AsNoTracking()
                .OrderByDescending(x => x.PromoId)
                .Where(x => x.PromoId == pp);
            foreach (var item in PP)
            {
                if(item.ProductId == id)
                {
                    return View(models);
                }
            }
            TblProductsPromotion tblProductsPromotion = new TblProductsPromotion { ProductId = id, PromoId = pp };
            _context.Add(tblProductsPromotion);
            await _context.SaveChangesAsync();
            return View(models);
        }

        // GET: admin/ProductsPromotions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblProductsPromotions == null)
            {
                return NotFound();
            }

            var tblProductsPromotion = await _context.TblProductsPromotions.FindAsync(id);
            if (tblProductsPromotion == null)
            {
                return NotFound();
            }
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId", tblProductsPromotion.ProductId);
            ViewData["PromoId"] = new SelectList(_context.TblPromotions, "PromoId", "PromoId", tblProductsPromotion.PromoId);
            return View(tblProductsPromotion);
        }

        // POST: admin/ProductsPromotions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PromoId,ProductId")] TblProductsPromotion tblProductsPromotion)
        {
            if (id != tblProductsPromotion.PpId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblProductsPromotion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductsPromotionExists(tblProductsPromotion.PpId))
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
            ViewData["ProductId"] = new SelectList(_context.TblProducts, "ProductId", "ProductId", tblProductsPromotion.ProductId);
            ViewData["PromoId"] = new SelectList(_context.TblPromotions, "PromoId", "PromoId", tblProductsPromotion.PromoId);
            return View(tblProductsPromotion);
        }

        // GET: admin/ProductsPromotions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblProductsPromotions == null)
            {
                return NotFound();
            }

            var tblProductsPromotion = await _context.TblProductsPromotions
                .Include(t => t.Product)
                .Include(t => t.Promo)
                .FirstOrDefaultAsync(m => m.PpId == id);
            if (tblProductsPromotion == null)
            {
                return NotFound();
            }

            return View(tblProductsPromotion);
        }

        // POST: admin/ProductsPromotions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblProductsPromotions == null)
            {
                return Problem("Entity set 'ecommerceContext.TblProductsPromotions'  is null.");
            }
            var tblProductsPromotion = await _context.TblProductsPromotions.FindAsync(id);
            int PromoId = tblProductsPromotion.PromoId;
            if (tblProductsPromotion != null)
            {
                _context.TblProductsPromotions.Remove(tblProductsPromotion);
            }
            
            await _context.SaveChangesAsync();
            return Redirect("Index/" + PromoId.ToString());
        }

        private bool TblProductsPromotionExists(int id)
        {
          return (_context.TblProductsPromotions?.Any(e => e.PpId == id)).GetValueOrDefault();
        }
    }
}
