namespace BackEndForFashion.Entities
{
    public enum Status
    {
        Pending = 0, Confirmed = 1, Shipped = 2, Delivered = 3, Cancelled = -1
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
