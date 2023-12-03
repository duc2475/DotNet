using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ecommerce.Models;
using PagedList.Core;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductsController : Controller
    {
        private readonly ecommerceContext _context;

        private readonly IWebHostEnvironment _hostEnviroment;

        public ProductsController(ecommerceContext context, IWebHostEnvironment hostEnviroment)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
        }

        // GET: admin/Products
        public IActionResult Index(int? page)
        {
            var pageNumber = page == null || page <= 0 ? 1 : page.Value;
            var pageSize = 20;
            var lsProduct = _context.TblProducts
                .AsNoTracking()
                .OrderByDescending(x => x.ProductId);
            PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
            ViewBag.CurrentPage = pageNumber;
            return View(models);
        }

        // GET: admin/Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts
                .Include(t => t.Ecat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // GET: admin/Products/Create
        public IActionResult Create()
        {
            ViewData["EcatId"] = new SelectList(_context.TblEndCategories, "EcatId", "EcatName");
            ViewData["ColorId"] = new SelectList(_context.TblColors, "ColorName", "ColorName");
            return View();
        }

        // POST: admin/Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPic,Color,StockQuantity,EcatId,ProductDes,ProductStatus,ProductPrice")] TblProduct tblProduct, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                tblProduct.ProductPic = fileName;
                string path = Path.Combine(wwwRootPath + "/assets/img/products/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                _context.Add(tblProduct);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EcatId"] = new SelectList(_context.TblEndCategories, "EcatId", "EcatName", tblProduct.EcatId.ToString());
            ViewData["ColorId"] = new SelectList(_context.TblColors, "ColorName", "ColorName", tblProduct.Color.ToString());
            return View(tblProduct);
        }

        // GET: admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts.FindAsync(id);
            if (tblProduct == null)
            {
                return NotFound();
            }
            ViewData["EcatId"] = new SelectList(_context.TblEndCategories, "EcatId", "EcatName", tblProduct.EcatId.ToString());
            ViewData["ColorId"] = new SelectList(_context.TblColors, "ColorName", "ColorName", tblProduct.Color.ToString());
            return View(tblProduct);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPic,Color,StockQuantity,EcatId,ProductDes,ProductStatus,ProductPrice")] TblProduct tblProduct, IFormFile? file)
        {
            if (id != tblProduct.ProductId)
            {
                return NotFound();
            }

            if(file != null)
            {
                string wwwRootPath = _hostEnviroment.WebRootPath;
                string fileName = Path.GetFileName(file.FileName);
                string extension = Path.GetExtension(file.FileName);
                tblProduct.ProductPic = fileName;
                string path = Path.Combine(wwwRootPath + "/assets/img/products/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                _context.Update(tblProduct);
                await _context.SaveChangesAsync();
            }else 
            {
                try
                {
                    _context.Update(tblProduct);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TblProductExists(tblProduct.ProductId))
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
            ViewData["EcatId"] = new SelectList(_context.TblEndCategories, "EcatId", "EcatName", tblProduct.EcatId.ToString());
            ViewData["ColorId"] = new SelectList(_context.TblColors, "ColorName", "ColorName", tblProduct.Color.ToString());
            return View(tblProduct);
        }

        // GET: admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TblProducts == null)
            {
                return NotFound();
            }

            var tblProduct = await _context.TblProducts
                .Include(t => t.Ecat)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (tblProduct == null)
            {
                return NotFound();
            }

            return View(tblProduct);
        }

        // POST: admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.TblProducts == null)
            {
                return Problem("Entity set 'ecommerceContext.TblProducts'  is null.");
            }
            var tblProduct = await _context.TblProducts.FindAsync(id);
            if (tblProduct != null)
            {
                _context.TblProducts.Remove(tblProduct);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TblProductExists(int id)
        {
          return (_context.TblProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
