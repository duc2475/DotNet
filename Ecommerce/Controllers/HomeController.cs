using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ecommerceContext _context;

        public HomeController(ILogger<HomeController> logger, ecommerceContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return _context.TblProductsPromotions != null? View(await _context.TblProductsPromotions
                .AsNoTracking()
                .Include(x =>x.Promo)
                .Include(x =>x.Product)
                .OrderByDescending(x => x.PpId)
                .Take(3).ToListAsync()) : Problem("Entity set 'ecommerceContext.TblProductsPromotions'  is null.");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}