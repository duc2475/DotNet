using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class SearchController : Controller
    {
        private readonly ecommerceContext _context;

        public SearchController(ecommerceContext context)
        {
			_context = context;
        }
        [HttpPost]
        public IActionResult FindProduct(string keyword)
        {
            List<TblProduct> tblProducts = new List<TblProduct>();
            if(string.IsNullOrEmpty(keyword)||keyword.Length < 1)
            {
                return PartialView("LisProductsSearchPartical", null);
            }
            tblProducts = _context.TblProducts
                .AsNoTracking()
                .Where(x => x.ProductName.Contains(keyword))
                .OrderByDescending(x => x.ProductName)
                .Take(10)
                .ToList();
            if(tblProducts == null)
            {
                return PartialView("LisProductsSearchPartical", null);
            }
            else
            {
                return PartialView("LisProductsSearchPartical", tblProducts);
            }
        }
    }
}
