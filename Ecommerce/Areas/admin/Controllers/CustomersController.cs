using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core.Mvc;
using PagedList.Core;
using AspNetCoreHero.ToastNotification.Abstractions;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class CustomersController : Controller
    {
        private readonly ecommerceContext _context;

        public INotyfService _notyfService { get; }

        public CustomersController(ecommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: admin/Customers
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsCustomer = _context.TblCustomers.AsNoTracking().OrderByDescending(x => x.CustId);
            PagedList<TblCustomer> models = new PagedList<TblCustomer>(lsCustomer, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblCustomers == null)
            {
                return NotFound();
            }

            var tblCustomer = await _context.TblCustomers
                .FirstOrDefaultAsync(m => m.CustId == id);
            if (tblCustomer == null)
            {
                return NotFound();
            }

            return View(tblCustomer);
        }

        // GET: admin/Customers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CustId,CustName,CustLname,CustEmail,CustPhone,CustAddress,CustCity,CustSate,CustZip,CustDatetime,CustStatus")] TblCustomer tblCustomer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblCustomer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblCustomer);
        }

        // GET: admin/Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblCustomers == null)
            {
                return NotFound();
            }

            var tblCustomer = await _context.TblCustomers.FindAsync(id);
            if (tblCustomer == null)
            {
                return NotFound();
            }
            return View(tblCustomer);
        }

        // POST: admin/Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CustId,CustName,CustLname,CustEmail,CustPhone,CustAddress,CustCity,CustSate,CustZip,CustDatetime,CustStatus")] TblCustomer tblCustomer)
        {
            if (id != tblCustomer.CustId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblCustomer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblCustomerExists(tblCustomer.CustId))
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
            return View(tblCustomer);
        }

        // GET: admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblCustomers == null)
            {
                return NotFound();
            }

            var tblCustomer = await _context.TblCustomers
                .FirstOrDefaultAsync(m => m.CustId == id);
            if (tblCustomer == null)
            {
                return NotFound();
            }

            return View(tblCustomer);
        }

        // POST: admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                if (_context.TblCustomers == null)
                {
                    return Problem("Entity set 'ecommerceContext.TblCustomers'  is null.");
                }
                var tblCustomer = await _context.TblCustomers.FindAsync(id);
                if (tblCustomer != null)
                {
                    _context.TblCustomers.Remove(tblCustomer);
                }

                await _context.SaveChangesAsync();
                _notyfService.Success("Xoá thành công");
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                _notyfService.Error("Xoá không thành công");
                return RedirectToAction(nameof(Index));
            }
            
        }

        private bool TblCustomerExists(int id)
        {
          return (_context.TblCustomers?.Any(e => e.CustId == id)).GetValueOrDefault();
        }
    }
}
