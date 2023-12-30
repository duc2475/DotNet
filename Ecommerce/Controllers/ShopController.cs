using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList.Core;

namespace Ecommerce.Controllers
{
	public class ShopController : Controller
	{
		private readonly ecommerceContext _context;

		public ShopController(ecommerceContext context)
		{
			_context = context;
		}
		[Route("shop.html", Name ="Shop")]
		public IActionResult Index(int page = 1, int type =  0)
		{
			if(type == 0)
			{
				var pageNumber = page;
				var pageSize = 16;
				var lsProduct = _context.TblProducts
					.AsNoTracking()
					.OrderByDescending(x => x.ProductId)
					.Include( x => x.TblProductsPromotions)
					.ThenInclude(x => x.Promo)
					.Where(x => x.ProductStatus == "Active");
				ViewBag.TCats = _context.TblTopCategories.AsNoTracking().OrderByDescending(x => x.TcatId);
				ViewBag.MCats = _context.TblMidCategories.AsNoTracking().OrderByDescending(x => x.McatId);
				ViewBag.ECats = _context.TblEndCategories.AsNoTracking().OrderByDescending(x => x.EcatId);
				PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
				ViewBag.CurrentPage = pageNumber;
				ViewBag.Type = _context.TblEndCategories.AsNoTracking().Include(x => x.Mcat).FirstOrDefault(x => x.EcatId == type);
				return View(models);
			}
			else
			{
				var pageNumber = page;
				var pageSize = 16;
				var lsProduct = _context.TblProducts
					.AsNoTracking()
					.Where(x => x.EcatId == type)
					.Include(x => x.TblProductsPromotions)
                    .ThenInclude(x => x.Promo)
                    .OrderByDescending(x => x.ProductId)
					.Where(x => x.ProductStatus == "Active");
				ViewBag.TCats = _context.TblTopCategories.AsNoTracking().OrderByDescending(x => x.TcatId);
				ViewBag.MCats = _context.TblMidCategories.AsNoTracking().OrderByDescending(x => x.McatId);
				ViewBag.ECats = _context.TblEndCategories.AsNoTracking().OrderByDescending(x => x.EcatId);
				PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
				ViewBag.CurrentPage = pageNumber;
				ViewBag.Type = _context.TblEndCategories.AsNoTracking().Include(x => x.Mcat).FirstOrDefault(x => x.EcatId == type);
				return View(models);
			}
		}
		[Route("/shop/{ProductSeo}-{ProductId}.html", Name = "Details")]
		public IActionResult Details(int? ProductId)
		{
			var product = _context.TblProducts
				.AsNoTracking()
				.Include(x =>x.Ecat)
				.Include(x => x.TblProductPics)
				.Include(x => x.TblProductsPromotions)
				.ThenInclude(x => x.Promo)
				.SingleOrDefault(x => x.ProductId == ProductId);
			if(product == null)
			{
				return RedirectToAction("Index");
			}
			return View(product);
		}
	}
}
