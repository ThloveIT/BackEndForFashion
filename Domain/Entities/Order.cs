using System.ComponentModel.DataAnnotations;

namespace BackEndForFashion.Domain.Entities
{
    public enum Status
    {
        [Display(Name = "Đã hủy")]
        Cancelled = -1,

        [Display(Name = "Chờ xác nhận")]
        Pending = 0,

        [Display(Name = "Đã xác nhận")]
        Confirmed = 1,

        [Display(Name = "Đang vận chuyển")]
        Shipped = 2,

        [Display(Name = "Đã giao hàng")]
        Delivered = 3
    }
    //luu thong tin don hang cua nguoi dung
    public class Order
    {
        public Guid Id { get; set; }
        //ma don hang
        public string OrderCode { get; set; }
        // tong gia tri don hang
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string ContactName { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhoneNumber { get; set; }
        //trang thai don hang
        public Status? Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //quan he
        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public Payment Payment { get; set; }
    }
}
