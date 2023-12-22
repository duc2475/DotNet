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

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class ProductsController : Controller
    {
        private readonly ecommerceContext _context;

        private readonly IWebHostEnvironment _hostEnviroment;

        public INotyfService _notyfService { get; }

        public ProductsController(ecommerceContext context, IWebHostEnvironment hostEnviroment, INotyfService notyfService)
        {
            _context = context;
            _hostEnviroment = hostEnviroment;
            _notyfService = notyfService;
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
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductPic,Color,StockQuantity,EcatId,ProductDes,ProductStatus,ProductPrice")] TblProduct tblProduct, IFormFile file, List<IFormFile> mfile)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnviroment.WebRootPath;
                    string fileName = Path.GetFileName(file.FileName);
                    string extension = Path.GetExtension(file.FileName);
                    tblProduct.ProductPic = fileName;
                    string path = Path.Combine(wwwRootPath + "/assets/img/products/", fileName);
                    tblProduct.ProductSeo = Extention.Extention.ToUrlFriendly(tblProduct.ProductName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    _context.Add(tblProduct);
                    await _context.SaveChangesAsync();
                    foreach (var item in mfile)
                    {
                        string mfileName = Path.GetFileName(item.FileName);
                        string mextension = Path.GetExtension(item.FileName);
                        TblProductPic productPic = new TblProductPic
                        {
                            PicName = mfileName,
                            ProductId = tblProduct.ProductId
                        };
                        string mpath = Path.Combine(wwwRootPath + "/assets/img/products/", mfileName);
                        using (var mfileStream = new FileStream(mpath, FileMode.Create))
                        {
                            await item.CopyToAsync(mfileStream);
                        }
                        _context.Add(productPic);
                        await _context.SaveChangesAsync();
                    }
                    
                    _notyfService.Success("Thêm mới thành công");
                    return RedirectToAction(nameof(Index));
                }
                catch
                {
                    _notyfService.Error("Thêm mới không thành công");
                    return RedirectToAction(nameof(Index));
                }
                
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
            ViewBag.Pic = _context.TblProductPics.AsNoTracking().OrderByDescending(x => x.ProductId == id);
            return View(tblProduct);
        }

        // POST: admin/Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,ProductPic,Color,StockQuantity,EcatId,ProductDes,ProductStatus,ProductPrice")] TblProduct tblProduct, IFormFile? file, IEnumerable<IFormFile> mfile)
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
                tblProduct.ProductSeo = Extention.Extention.ToUrlFriendly(tblProduct.ProductName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                _context.Update(tblProduct);
                await _context.SaveChangesAsync();
                _notyfService.Success("Sửa thành công");
            }else 
            {
                try
                {
                    if(mfile != null)
                    {
                        foreach (var item in mfile)
                        {
                            string wwwRootPath = _hostEnviroment.WebRootPath;
                            string mfileName = Path.GetFileName(item.FileName);
                            string mextension = Path.GetExtension(item.FileName);
                            TblProductPic productPic = new TblProductPic
                            {
                                PicName = mfileName,
                                ProductId = tblProduct.ProductId
                            };
                            string mpath = Path.Combine(wwwRootPath + "/assets/img/products/", mfileName);
                            using (var mfileStream = new FileStream(mpath, FileMode.Create))
                            {
                                await item.CopyToAsync(mfileStream);
                            }
                            _context.Add(productPic);
                            await _context.SaveChangesAsync();
                        }
                        tblProduct.ProductSeo = Extention.Extention.ToUrlFriendly(tblProduct.ProductName);
                        _context.Update(tblProduct);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        tblProduct.ProductSeo = Extention.Extention.ToUrlFriendly(tblProduct.ProductName);
                        _context.Update(tblProduct);
                        await _context.SaveChangesAsync();
                    }
                    
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
            try
            {
                if (_context.TblProducts == null)
                {
                    return Problem("Entity set 'ecommerceContext.TblProducts'  is null.");
                }
                var tblProduct = await _context.TblProducts.FindAsync(id);
                if (tblProduct != null)
                {
                    var piclist = _context.TblProductPics.AsNoTracking().OrderByDescending(p => p.ProductId);
                    foreach (var pic in piclist)
                    {
                        _context.TblProductPics.Remove(pic);
                    }
                    _context.TblProducts.Remove(tblProduct);
                }

                await _context.SaveChangesAsync();
                _notyfService.Success("Xoá thành công");
                return RedirectToAction(nameof(Index));
            }catch (Exception ex)
            {
                _notyfService.Error("Xoá không thành công");
                return RedirectToAction(nameof(Index));
            }
           
        }

        private bool TblProductExists(int id)
        {
          return (_context.TblProducts?.Any(e => e.ProductId == id)).GetValueOrDefault();
        }
    }
}
