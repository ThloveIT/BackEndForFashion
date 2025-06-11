using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Application.ViewModels
{
    public class UpdateUserVM
    {
        public Guid Id { get; set; } // Gán từ token, không để frontend gửi tự do
        [Required(ErrorMessage = "Không được để trống username")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Không được để trống email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
    }
}
