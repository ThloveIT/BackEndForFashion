namespace BackEndForFashion.Domain.Entities
{
    public enum PaymentMethod
    {
        Online = 0,
        COD = 1,
    }
    public enum PaymentStatus
    {
        //Cho thanh toan
        Pending = 0,
        //Da thanh toan
        Completed = 1,
        //Thanh toan that bai
        Failed = -1
    }
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        //so tien thanh toan
        public decimal Amount { get; set; }
        //phuong thuc thanh toan
        public PaymentMethod PaymentMethod { get; set; }
        public PaymentStatus Status { get; set; }
        //thoi gian thuc hien thanh toan
        public DateTime CreatedAt { get; set; }

        //quan he 
        public Order Order { get; set; }
    }
}
