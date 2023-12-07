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

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class UsersController : Controller
    {
        private readonly ecommerceContext _context;

        public UsersController(ecommerceContext context)
        {
            _context = context;
        }

        // GET: admin/Users
        public  IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsAccount = _context.TblUsers.AsNoTracking().OrderByDescending(x => x.UserId);
            PagedList<TblUser> models = new PagedList<TblUser>(lsAccount, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: admin/Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblUsers == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tblUser == null)
            {
                return NotFound();
            }

            return View(tblUser);
        }

        // GET: admin/Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,UserEmail,UserPhone,UserPass,UserPhoto,UserRole,UserStatus")] TblUser tblUser)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tblUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblUser);
        }

        // GET: admin/Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblUsers == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUsers.FindAsync(id);
            if (tblUser == null)
            {
                return NotFound();
            }
            return View(tblUser);
        }

        // POST: admin/Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,UserEmail,UserPhone,UserPass,UserPhoto,UserRole,UserStatus")] TblUser tblUser)
        {
            if (id != tblUser.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tblUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblUserExists(tblUser.UserId))
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
            return View(tblUser);
        }

        // GET: admin/Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblUsers == null)
            {
                return NotFound();
            }

            var tblUser = await _context.TblUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (tblUser == null)
            {
                return NotFound();
            }

            return View(tblUser);
        }

        [HttpPost, ActionName("Active")]
        public async Task<IActionResult> Active(int id)
        {

            var tblUser = await _context.TblUsers
                .FirstOrDefaultAsync(m => m.UserId == id);

            if (tblUser != null)
            {
                tblUser.UserStatus = "Active";
                _context.Update(tblUser);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        //Post: admin/Users/Unactive/5
        [HttpPost, ActionName("Unactive")]
        public async Task<IActionResult> Unactive(int id)
        {

            var tblUser = await _context.TblUsers
                .FirstOrDefaultAsync(m => m.UserId == id);
            
            if (tblUser != null)
            {
                tblUser.UserStatus = "Unactive";
                _context.Update(tblUser);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: admin/Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblUsers == null)
            {
                return Problem("Entity set 'ecommerceContext.TblUsers'  is null.");
            }
            var tblUser = await _context.TblUsers.FindAsync(id);
            if (tblUser != null)
            {
                _context.TblUsers.Remove(tblUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblUserExists(int id)
        {
          return (_context.TblUsers?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
