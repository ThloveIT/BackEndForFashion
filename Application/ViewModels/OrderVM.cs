using BackEndForFashion.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Application.ViewModels
{
    public class OrderVM
    {
        public Guid Id { get; set; }
        public string? OrderCode { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public string ShippingAddress { get; set; }
        // Họ tên người đặt hàng
        [Required]
        public string ContactName { get; set; }
        [Required]
        [EmailAddress]
        // Email người đặt hàng
        public string ContactEmail { get; set; }
        // Số điện thoại người đặt hàng
        [Required]
        public string ContactPhoneNumber { get; set; }
        public string? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public Guid UserId { get; set; }
        // Phương thức thanh toán
        [Required]
        public PaymentMethod PaymentMethod { get; set; }
        [Required]
        public List<OrderDetailVM> OrderDetails { get; set; }
    }
}
