using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.admin.Controllers
{
    [Area("admin")]
    public class HomeController : Controller
    {
        private readonly ecommerceContext _context; 
        public HomeController( ecommerceContext context) 
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var taikhoanId = HttpContext.Session.GetString("user_id");
            var taikhoan = _context.TblUsers.AsNoTracking().SingleOrDefault(x => x.UserId == Convert.ToInt32(taikhoanId));
            if (taikhoanId == null || taikhoan.UserRole != "admin")
            {
                return Redirect("/");
            }
            ViewBag.DonHangs = _context.TblCarts.AsNoTracking().OrderByDescending(x => x.CartId);
            ViewBag.Users = _context.TblUsers.AsNoTracking().OrderByDescending(x => x.UserRole != "admin");
            ViewBag.Discount = _context.TblPromotions.AsNoTracking().OrderByDescending(x => x.PromoId);
            return View(taikhoan);
        }
    }
}
