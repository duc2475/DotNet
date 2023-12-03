using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using System.Drawing;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class PostersController : Controller
    {
        private readonly ecommerceContext _context;

        private readonly IWebHostEnvironment _hostEnviroment;

        public PostersController(ecommerceContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnviroment = hostEnvironment;
        }

        // GET: admin/Posters
        public async Task<IActionResult> Index()
        {
              return _context.TblPosters != null ? 
                          View(await _context.TblPosters.ToListAsync()) :
                          Problem("Entity set 'ecommerceContext.TblPosters'  is null.");
        }

        // GET: admin/Posters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblPosters == null)
            {
                return NotFound();
            }

            var tblPoster = await _context.TblPosters
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tblPoster == null)
            {
                return NotFound();
            }

            return View(tblPoster);
        }

        // GET: admin/Posters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/Posters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        /*[ValidateAntiForgeryToken]*/
        public async Task<IActionResult> Create(string PostTile,IFormFile PostPic)
        {
            TblPoster tblPoster = new TblPoster();
            if(PostTile != null && PostPic != null)
            {
                tblPoster.PostTile = PostTile;
                string wwwRootPath = _hostEnviroment.WebRootPath;
                string fileName = Path.GetFileName(PostPic.FileName);
                string extension = Path.GetExtension(PostPic.FileName);
                tblPoster.PostName = fileName;
                string path = Path.Combine(wwwRootPath + "/assets/img/poster/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await PostPic.CopyToAsync(fileStream);
                }
                _context.Add(tblPoster);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tblPoster);
        }

        // GET: admin/Posters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblPosters == null)
            {
                return NotFound();
            }

            var tblPoster = await _context.TblPosters.FindAsync(id);
            if (tblPoster == null)
            {
                return NotFound();
            }
            return View(tblPoster);
        }

        // POST: admin/Posters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PostId,PostTile,PostName")] TblPoster tblPoster, IFormFile PostPic)
        {
            if (id != tblPoster.PostId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnviroment.WebRootPath;
                    string fileName = Path.GetFileName(PostPic.FileName);
                    string extension = Path.GetExtension(PostPic.FileName);
                    tblPoster.PostName = fileName;
                    string path = Path.Combine(wwwRootPath + "/assets/img/poster/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await PostPic.CopyToAsync(fileStream);
                    }
                    _context.Update(tblPoster);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblPosterExists(tblPoster.PostId))
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
            return View(tblPoster);
        }

        // GET: admin/Posters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblPosters == null)
            {
                return NotFound();
            }

            var tblPoster = await _context.TblPosters
                .FirstOrDefaultAsync(m => m.PostId == id);
            if (tblPoster == null)
            {
                return NotFound();
            }

            return View(tblPoster);
        }

        // POST: admin/Posters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblPosters == null)
            {
                return Problem("Entity set 'ecommerceContext.TblPosters'  is null.");
            }
            var tblPoster = await _context.TblPosters.FindAsync(id);
            if (tblPoster != null)
            {
                _context.TblPosters.Remove(tblPoster);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblPosterExists(int id)
        {
          return (_context.TblPosters?.Any(e => e.PostId == id)).GetValueOrDefault();
        }
    }
}
