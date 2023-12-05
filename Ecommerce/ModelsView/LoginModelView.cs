using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ModelsView
{
	public class LoginModelView
	{
		[Key]
		[MaxLength(100)]
		[Required(ErrorMessage ="Vui lòng nhập đúng tên đăng nhập")]
		[Display(Name = "Tên đăng nhập")]
		public string UserName { get; set; }

		[MinLength(5, ErrorMessage ="Bạn cần đặt ít nhất 5 kí tự")]
		[Required(ErrorMessage = "Vui lòng nhập đúng mật khẩu")]
		[Display(Name = "Mật Khẩu")]
		public string Password { get; set; }
	}
}
