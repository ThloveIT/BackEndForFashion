using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Application.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage ="Không được để trống username")]
        public string UserName { get; set; }
        [Required(ErrorMessage ="Không được để trống email")]
        [EmailAddress(ErrorMessage ="Email không hợp lệ")]
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage ="Không được để trống")]
        [MinLength(6, ErrorMessage ="Mật khẩu ít nhất 6 ký tự")]
        public string Password { get; set; }
        public string Address { get; set; }
    }
}
