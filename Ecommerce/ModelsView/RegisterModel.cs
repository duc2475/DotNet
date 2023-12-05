using System.ComponentModel.DataAnnotations;

namespace Ecommerce.ModelsView
{
	public class RegisterModel
	{
		[Key]
		public int UserId { get; set; }
		[Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
		[Display(Name = "Tên đăng nhập")]
		public string UserName { get; set; } = null!;
		[Required(ErrorMessage = "Vui lòng nhập Email")]
		[Display(Name = "Email")]
		public string UserEmail { get; set; } = null!;
		[Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
		[Display(Name = "Số điện thoại")]
		public string UserPhone { get; set; } = null!;
		[Required(ErrorMessage = "Vui lòng nhập nhập mật khẩu")]
		[Display(Name = "Mật khẩu")]
		public string UserPass { get; set; } = null!;
		[Required(ErrorMessage = "Vui lòng nhập đúng mật khẩu")]
		[Display(Name = "Xác nhận mật khẩu")]
		public string ConfirmPassword { get; set; }
		public string UserPhoto { get; set; } = null!;
	}
}
