using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Extention;
using Ecommerce.Models;
using Ecommerce.ModelsView;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Ecommerce.Controllers
{
	[Authorize]
	public class AccountsController : Controller
	{
		private readonly ecommerceContext _context;

		public INotyfService _notyfService;

		private readonly IWebHostEnvironment _hostEnviroment;

		public AccountsController(ecommerceContext context, IWebHostEnvironment hostEnvironment, INotyfService notyfService)
		{
			_hostEnviroment = hostEnvironment;
			_context = context;
			_notyfService = notyfService;
		}
		[Route("tai-khoan-cua-toi.html", Name ="Index")]
		public IActionResult Index()
		{
			var user = HttpContext.Session.GetString("user_id");
			if(user != null) 
			{
				var khachhang = _context.TblUsers.AsNoTracking().SingleOrDefault(x => x.UserId == Convert.ToInt32(user));
				if(khachhang != null)
				{
					var diachi = _context.TblUserCustomers.AsNoTracking().FirstOrDefault(x => x.UserId == khachhang.UserId);

                    if (diachi != null)
					{
						ViewBag.DiaChi = _context.TblCustomers.AsNoTracking().FirstOrDefault(x => x.CustId == diachi.CustId);
					}
					else
					{
						ViewBag.DiaChi = null;
					}
					return View(khachhang);
				}
			}
			return RedirectToAction("Login","Accounts");
		}
		[HttpGet]
		[AllowAnonymous]
		[Route("dang-ky.html", Name = "DangKy")]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		[AllowAnonymous]
		[Route("dang-ky.html", Name = "DangKy")]
		public async Task<IActionResult> Register(RegisterModel User, IFormFile file)
		{
			if (User != null)
			{
				string wwwRootPath = _hostEnviroment.WebRootPath;
				string fileName = Path.GetFileName(file.FileName);
				string extension = Path.GetExtension(file.FileName);
				string path = Path.Combine(wwwRootPath + "/assets/img/users/", fileName);
				using (var fileStream = new FileStream(path, FileMode.Create))
				{
					await file.CopyToAsync(fileStream);
				}
				TblUser tblUser = new TblUser
				{
					UserName = User.UserName,
					UserEmail = User.UserEmail.Trim().ToLower(),
					UserPhone = User.UserPhone.Trim().ToLower(),
					UserPass = (User.UserPass).ToMD5(),
					UserPhoto = fileName,
					UserRole = "user",
					UserStatus = "Active"
				};
				try
				{
					_context.Add(tblUser);
					await _context.SaveChangesAsync();

					HttpContext.Session.SetString("user_id", tblUser.UserId.ToString());
					var taikhoanId = HttpContext.Session.GetString("user_id");

					var claim = new List<Claim>
					{
						new Claim(ClaimTypes.Name, tblUser.UserName),
						new Claim("user_id", tblUser.UserId.ToString())
					};

					ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, "login");
					ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
					await HttpContext.SignInAsync(claimsPrincipal);
					return RedirectToAction("Index","Accounts");

				}catch (Exception ex)
				{
					return RedirectToAction("Register", "Accounts");
				}

			}
			else
			{
				return View(User);
			}
		}
		[AllowAnonymous]
		[Route("dang-nhap.html", Name ="DangNhap")]
		public IActionResult Login(string returnUrl = null)
		{
			var taiKhoanId = HttpContext.Session.GetString("user_id");
			if(taiKhoanId != null)
			{
				return RedirectToAction("Index", "Home");
			}
			return View();
		}
		[HttpPost]
        [AllowAnonymous]
        [Route("dang-nhap.html", Name = "DangNhap")]
        public async Task<IActionResult> Login(LoginModelView login, string returnUrl)
		{
			try
			{
				if (login != null)
				{
					var user = _context.TblUsers.AsNoTracking().SingleOrDefault(x => x.UserName.Trim() == login.UserName);
					if (user == null)
					{
						return RedirectToAction("Register", "Accounts");
					}
					string pass = login.Password.ToMD5();
					if (pass != user.UserPass)
					{
						_notyfService.Error("Sai thông tin đăng nhập");
						return View();
					}
					if(user.UserStatus == "Active")
					{
						HttpContext.Session.SetString("user_id", user.UserId.ToString());
						var taikhoanId = HttpContext.Session.GetString("user_id");

						var claim = new List<Claim>
						{
						new Claim(ClaimTypes.Name, user.UserName),
						new Claim("user_id", user.UserId.ToString())
						};

						ClaimsIdentity claimsIdentity = new ClaimsIdentity(claim, "login");
						ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
						await HttpContext.SignInAsync(claimsPrincipal);
						_notyfService.Success("Đăng Nhập Thành Công!");
						if(user.UserRole == "admin")
						{
							return Redirect("/admin");
						}
						return RedirectToAction("Index", "Home");
					}
				}
			}
			catch (Exception ex)
			{
				return RedirectToAction("Register", "Accounts");
			}
			return View();
		}
		[HttpGet]
		[Route("dang-xuat.html", Name = "DangXuat")]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync();
			HttpContext.Session.Remove("user_id");
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		public async Task<IActionResult> Address(int id,string CustName, string CustLname, string CustEmail, string CustPhone, string CustAddress, string CustCity, string CustSate, string CustZip, string CustDatetime, int CustStatus)
		{
			TblCustomer customer = new TblCustomer
			{
				CustName = CustName,
				CustLname = CustLname,
				CustEmail = CustEmail,
				CustPhone = CustPhone,
				CustAddress = CustAddress,
				CustCity = CustCity,
				CustSate = CustSate,
				CustZip = CustZip,
				CustDatetime = CustDatetime,
				CustStatus = CustStatus
			};
			try
			{
				_context.Add(customer);
				await _context.SaveChangesAsync();
				TblUserCustomer userCustomer = new TblUserCustomer
				{
					UserId = id,
					CustId = customer.CustId
				};
				_context.Add(userCustomer);
                await _context.SaveChangesAsync();
				_notyfService.Success("Cập nhật thành công");
				return RedirectToAction("Index");
            }catch (Exception ex)
			{
				_notyfService.Error("Cập nhật thất bại");
				return RedirectToAction("Index");
			}
		}
        [HttpPost]
        public async Task<IActionResult> FixAddress(int id, string CustName, string CustLname, string CustEmail, string CustPhone, string CustAddress, string CustCity, string CustSate, string CustZip, string CustDatetime, int CustStatus)
        {
			var customer = _context.TblCustomers.AsNoTracking().SingleOrDefault(x => x.CustId == id);
			if(customer != null)
			{
				_context.Update(customer);
                await _context.SaveChangesAsync();
                _notyfService.Success("Cập nhật thành công");
                return RedirectToAction("Index");
			}
			else
			{
				_notyfService.Error("Cập nhật không thành công");
				return RedirectToAction("Index");

            }
        }
    }
}
