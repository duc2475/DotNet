using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extention;
using Ecommerce.Models;
using Ecommerce.ModelsView;
using Ecommerce.PayPal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading;
using System.Xml.Linq;

namespace Ecommerce.Controllers
{
    public class CheckoutController : Controller
    {
        public readonly ecommerceContext _context;

        private readonly SendMailService _sendMailService;

        public INotyfService _notyfService;

        private readonly PaypalClient _paypalClient;
        public CheckoutController(ecommerceContext context, INotyfService notyfService, SendMailService sendMailService, PaypalClient paypalClient)
        {
            _context = context;
            _notyfService = notyfService;
            _sendMailService = sendMailService;
            _paypalClient = paypalClient;
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
            ViewBag.PaypalClientId = _paypalClient.ClientId;
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

                    var mailContent = new MailContent();
                    mailContent.To = khachang.Cust.CustEmail;
                    mailContent.Subject = "Xác Nhận Đặt Hàng Thành Công - FamilyBao";

                    string add = "";

                    foreach(var item in cart)
                    {
                        add += "<tr style=\"height: 50px;\"><td style=\"text-align: center;\">" + item.amount + "</td><td style=\"text-align: center;\">" + item.Product.ProductName + "</td><td style=\"text-align: center;\">" + item.Product.ProductPrice.Value.ToString("#,##0") + "VNĐ</td><td style=\"text-align: center;\">" + item.totalMoney.ToString("#,##0") + "VNĐ</td><tr>";
                    };

                    add += "</tbody></table>";

                    string messBody = "<table style=\"margin-left: auto; margin-right: auto;\"><thead style=\"background-color: #F0F0F0;\"><tr style=\"height: 50px;\"><th style=\"width: 10%; text-align: center;\">Số Lượng</th><th style=\"width: 50%; text-align: center;\">Sản Phẩm</th><th style=\"width: 20%; text-align: center;\">Đơn Giá</th><th style=\"width: 20%;text-align: center;\">Tổng Cộng</th></tr></thead>" + "<tbody>"+ add;

                    mailContent.Body = messBody;

                    await _sendMailService.SendMail(mailContent);
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

        double ChuyenDoiTienTe(double soTienVND)
        {
            // Tỉ giá USD/VND hiện tại là 23,000 VND[^1^][1]
            double tiGia = 24283;
            // Chuyển đổi bằng cách chia số tiền VND cho tỉ giá
            double soTienUSD = soTienVND / tiGia;
            // Trả về kết quả
            return soTienUSD;
        }

        [Authorize]
        [HttpPost("/Checkout/create-paypal-order")]
        public async Task<IActionResult> CreatePaypalOrder(CancellationToken cancellationToken)
        {
            try
            {
                var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
                var tong = cart.Sum(x => x.totalMoney);
                var TongTien = ChuyenDoiTienTe(tong);
                var DonViTienTe = "USD";
                var MaDonHang = "DH" + DateTime.Now.Ticks.ToString();

                var response = await _paypalClient.CreateOrder(Math.Round(TongTien, 2).ToString(), DonViTienTe, MaDonHang);
                return Ok(response);
            }
            catch(Exception ex)
            {
                var error = new {ex.GetBaseException().Message};
                return BadRequest(error);
            }
            
        }
        [Authorize]
        [HttpPost("/Checkout/capture-paypal-order")]
        public async Task<IActionResult> CapturePaypalOrder(string orderID, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _paypalClient.CaptureOrder(orderID);
                return Ok(response);
            }
            catch (Exception ex)
            {
                var error = new { ex.GetBaseException().Message };
                return BadRequest(error);
            }
        }

        [Route("thankyou-paypal-payment.html", Name = "ThankYouPayMent")]
        public async Task<IActionResult> ThankYouPayMent()
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
                        CartStatus = 2
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

                    var mailContent = new MailContent();
                    mailContent.To = khachang.Cust.CustEmail;
                    mailContent.Subject = "Xác Nhận Đặt Hàng Thành Công - FamilyBao";

                    string add = "";

                    foreach (var item in cart)
                    {
                        add += "<tr style=\"height: 50px;\"><td style=\"text-align: center;\">" + item.amount + "</td><td style=\"text-align: center;\">" + item.Product.ProductName + "</td><td style=\"text-align: center;\">" + item.Product.ProductPrice.Value.ToString("#,##0") + "VNĐ</td><td style=\"text-align: center;\">" + item.totalMoney.ToString("#,##0") + "VNĐ</td><tr>";
                    };

                    add += "</tbody></table>";

                    string messBody = "<table style=\"margin-left: auto; margin-right: auto;\"><thead style=\"background-color: #F0F0F0;\"><tr style=\"height: 50px;\"><th style=\"width: 10%; text-align: center;\">Số Lượng</th><th style=\"width: 50%; text-align: center;\">Sản Phẩm</th><th style=\"width: 20%; text-align: center;\">Đơn Giá</th><th style=\"width: 20%;text-align: center;\">Tổng Cộng</th></tr></thead>" + "<tbody>" + add;

                    mailContent.Body = messBody;

                    await _sendMailService.SendMail(mailContent);
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
