using AspNetCoreHero.ToastNotification.Abstractions;
using Ecommerce.Models;
using Ecommerce.ModelsView;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ecommerce.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ecommerceContext _context;

        private readonly SendMailService _sendMailService;

        public INotyfService _notyfService;
        public HomeController(ILogger<HomeController> logger, ecommerceContext context, SendMailService sendMailService, INotyfService notyfService)
        {
            _logger = logger;
            _context = context;
            _sendMailService = sendMailService;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> Index()
        {
            return _context.TblProductsPromotions != null? View(await _context.TblProductsPromotions
                .AsNoTracking()
                .Include(x =>x.Promo)
                .Include(x =>x.Product)
                .OrderByDescending(x => x.PpId)
                .Take(11)
                .ToListAsync()) : Problem("Entity set 'ecommerceContext.TblProductsPromotions'  is null.");
        }

		public IActionResult About()
		{
            return View();
		}

        public IActionResult Services() 
        { 
            
            return View();
        }

        [Route("Contact.html", Name ="Contact")]
        public IActionResult Contact()
        {
            return View();
        }

        public async Task<IActionResult> SendMail(string fname, string lname, string email, string message)
        {
            try
            {
                var mailContent = new MailContent();
                mailContent.To = "custombanphim@gmail.com";
                mailContent.Subject = lname + " " + fname + " " + email + " " + "Contact!";
                mailContent.Body = message;
                await _sendMailService.SendMail(mailContent);
                _notyfService.Success("Gửi mail thành công");
                return RedirectToAction("Contact");
            }
            catch(Exception ex)
            {
                _notyfService.Error(ex.Message);
                return RedirectToAction("Contact");
            }
            
        }

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}