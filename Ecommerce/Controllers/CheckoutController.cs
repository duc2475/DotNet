using Ecommerce.Extention;
using Ecommerce.Models;
using Ecommerce.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class CheckoutController : Controller
    {
        public readonly ecommerceContext _context;

        public CheckoutController(ecommerceContext context)
        {
            _context = context;
        }

        [Route("checkout.html", Name ="Checkout")]
        public IActionResult Index(string returnUrl = null)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taiKhoanId = HttpContext.Session.GetString("user_id");
            var khachang = _context.TblUserCustomers
                .AsNoTracking()
                .Include(x => x.Cust)
                .SingleOrDefault(x => x.UserId.ToString() == taiKhoanId);
            ViewBag.Cart = cart;
            if(khachang != null)
            {
                return View(khachang);
            }
            else
            {
                return RedirectToAction("Index","Accounts");
            }
        }
    }
}
