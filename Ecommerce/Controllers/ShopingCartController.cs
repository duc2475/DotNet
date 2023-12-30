using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extention;
using Ecommerce.Models;
using Ecommerce.ModelsView;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Controllers
{
    public class ShopingCartController : Controller
    {
        private readonly ecommerceContext _context;

        public INotyfService _notyfService;

        public ShopingCartController(ecommerceContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        } 

        public List<CartItem> GioHang
        {
            get
            {
                var gh = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if(gh == default(List<CartItem>))
                {
                    gh = new List<CartItem>();
                }
                return gh;
            }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productId, int? amount)
        {
            List<CartItem> gioHang = GioHang;
            try
            {
                TblProduct product = _context.TblProducts.AsNoTracking().SingleOrDefault(x => x.ProductId == productId);
				CartItem item = GioHang.SingleOrDefault(x => x.Product.ProductId == productId);
				if (item != null)
                {
                    if (amount.HasValue)
                    {
                        item.amount = amount.Value;
						gioHang.SingleOrDefault(x => x.Product.ProductId == productId).amount = item.amount;
					}
                    else if(product.StockQuantity > 0 && item.amount < product.StockQuantity)
                    {
                        item.amount++;
						gioHang.SingleOrDefault(x => x.Product.ProductId == productId).amount = item.amount;
                    }
                    
                }
                else
                {
                    TblProduct tblProduct = _context.TblProducts.Include(x=>x.TblProductsPromotions)
                        .ThenInclude(x=>x.Promo)
                        .SingleOrDefault(x => x.ProductId == productId);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        Product = tblProduct
                    };
                    gioHang.Add(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                _notyfService.Success("Thêm giỏ hàng thành công");
                return Json(new {success = true });
            }catch 
            {
				return Json(new { success = false });
			}
            
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productId, int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if(cart != null)
                {
                    CartItem item = cart.SingleOrDefault(x => x.Product.ProductId == productId);
                    if(item != null && amount.HasValue)
                    {
                        if(amount == 0)
                        {
                            Remove(productId);
                        }
                        else
                        {
                            item.amount = amount.Value; 
                            HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                        }
                    }
                    
                }
                return Json(new { success = true });
            }catch

            { return Json(new { success = false });}
        }

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productId)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(x => x.Product.ProductId == productId);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
        }


        [Route("cart.html", Name ="Cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }
    }
}
