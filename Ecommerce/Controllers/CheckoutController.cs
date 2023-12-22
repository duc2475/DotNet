using AspNetCoreHero.ToastNotification.Abstractions;
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

        public INotyfService _notyfService;
        public CheckoutController(ecommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
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
            if(khachang != null && cart != null)
            {
                _notyfService.Error("Giỏ Hàng Của Bạn Đang Trống");
                return View(khachang);
            }
            else
            {
                return RedirectToAction("Index","Accounts");
            }
        }
        [Route("thankyou.html", Name = "ThankYou")]
        public async Task<IActionResult> ThankYou()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            var taiKhoanId = HttpContext.Session.GetString("user_id");
            var khachang = _context.TblUserCustomers
                .AsNoTracking()
                .Include(x => x.Cust)
                .SingleOrDefault(x => x.UserId.ToString() == taiKhoanId);
            List<TblCartDetail> cartDetails = new List<TblCartDetail>();
            try
            {
                if (khachang != null)
                {
                    TblCart tblCart = new TblCart
                    {
                        CartDate = DateTime.Now.ToString(),
                        TotalPrice = cart.Sum(x => x.totalMoney).ToString(),
                        CustId = khachang.CustId,
                        CartStatus = 0
                    };
                    _context.Add(tblCart);
                    await _context.SaveChangesAsync();
                    foreach (var item in cart)
                    {
                        TblCartDetail cartDetail = new TblCartDetail
                        {
                            CartId = tblCart.CartId,
                            ProductId = item.Product.ProductId,
                            ProductName = item.Product.ProductName,
                            ProductPic = item.Product.ProductPic,
                            Color = item.Product.Color,
                            Quantity = item.amount
                        };
                        cartDetails.Add(cartDetail);
                        var product = _context.TblProducts.AsNoTracking().SingleOrDefault(x => x.ProductId == item.Product.ProductId);
                        product.StockQuantity = product.StockQuantity - item.amount;
                        _context.Update(product);
                    }
                    _context.AddRange(cartDetails);
                    await _context.SaveChangesAsync();
                    _notyfService.Success("Đặt Hàng Thành Công");
                    HttpContext.Session.Remove("GioHang");
                    return View();
                }
                else
                {
                    _notyfService.Error("Đặt Hàng Không Thành Công");
                    return RedirectToAction("Index", "Accounts");
                }
            }
            catch
            {
                _notyfService.Error("Đặt Hàng Không Thành Công");
                return RedirectToAction("Index", "Accounts");
            }
            
        }
    }
}
