using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class CartsController : Controller
    {
        private readonly ecommerceContext _context;

        public INotyfService _notyfService { get; }

        public CartsController(ecommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: admin/Carts
        public async Task<IActionResult> Index()
        {
            var ecommerceContext = _context.TblCarts.Include(t => t.Cust);
            return View(await ecommerceContext.ToListAsync());
        }

        // GET: admin/Carts/Details/5
        public  IActionResult Details(int? id)
        {
            if (id == null || _context.TblCarts == null)
            {
                return NotFound();
            }

            ViewBag.Cart = _context.TblCarts.AsNoTracking().Include(x=>x.Cust).SingleOrDefault(x => x.CartId == id);

            var tblCart = _context.TblCartDetails
                .Include(x => x.Cart)
                .Include(x => x.Product)
                .OrderByDescending(x => x.CartId == id);

            if (tblCart == null)
            {
                return NotFound();
            }

            return View(tblCart);
        }

        // GET: admin/Carts/Create
        public IActionResult Create()
        {
            ViewData["CustId"] = new SelectList(_context.TblCustomers, "CustId", "CustId");
            return View();
        }

        // POST: admin/Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CartId,CartDate,TotalPrice,CustId")] TblCart tblCart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblCart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CustId"] = new SelectList(_context.TblCustomers, "CustId", "CustId", tblCart.CustId);
            return View(tblCart);
        }

        // GET: admin/Carts/Edit/5
        [ActionName("UpdateStatus")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                var tblCart = await _context.TblCarts.FindAsync(id);
                if (tblCart == null)
                {
                    _notyfService.Error("Cập nhật thất bại");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    tblCart.CartStatus = 1;
                    _context.Update(tblCart);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Cập nhật thành công");
                    return RedirectToAction(nameof(Index));
                }
                
            }
            catch (Exception ex)
            {
                _notyfService.Error("Cập nhật thất bại");
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: admin/Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CartId,CartDate,TotalPrice,CustId")] TblCart tblCart)
        {
            if (id != tblCart.CartId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCart);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCartExists(tblCart.CartId))
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
            ViewData["CustId"] = new SelectList(_context.TblCustomers, "CustId", "CustId", tblCart.CustId);
            return View(tblCart);
        }

        // GET: admin/Carts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblCarts == null)
            {
                return NotFound();
            }

            var tblCart = await _context.TblCarts
                .Include(t => t.Cust)
                .FirstOrDefaultAsync(m => m.CartId == id);
            if (tblCart == null)
            {
                return NotFound();
            }

            return View(tblCart);
        }

        // POST: admin/Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblCarts == null)
            {
                return Problem("Entity set 'ecommerceContext.TblCarts'  is null.");
            }
            var tblCart = await _context.TblCarts.FindAsync(id);
            if (tblCart != null)
            {
                _context.TblCarts.Remove(tblCart);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblCartExists(int id)
        {
          return (_context.TblCarts?.Any(e => e.CartId == id)).GetValueOrDefault();
        }
    }
}
