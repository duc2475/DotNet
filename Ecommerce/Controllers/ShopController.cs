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
		public IActionResult Index(int? page)
		{
			var pageNumber = page == null || page <= 0 ? 1 : page.Value;
			var pageSize = 20;
			var lsProduct = _context.TblProducts
				.AsNoTracking()
				.Include(x => x.TblProductsPromotions)
				.OrderByDescending(x => x.ProductId)
				.Where(x => x.ProductStatus == "Active");
			PagedList<TblProduct> models = new PagedList<TblProduct>(lsProduct, pageNumber, pageSize);
			ViewBag.CurrentPage = pageNumber;
			return View(models);
		}
		[Route("/shop/{ProductSeo}-{ProductId}.html", Name = "Details")]
		public IActionResult Details(int? ProductId)
		{
			var product = _context.TblProducts
				.AsNoTracking()
				.Include(x =>x.Ecat)
				.SingleOrDefault(x => x.ProductId == ProductId);
			if(product == null)
			{
				return RedirectToAction("Index");
			}
			return View(product);
		}
	}
}
