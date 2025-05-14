namespace BackEndForFashion.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        //so tien thanh toan
        public decimal Amount { get; set; }
        //phuong thuc thanh toan
        public string PaymentMethod { get; set; }
        public string Status { get; set; }
        //thoi gian thuc hien thanh toan
        public DateTime CreatedAt { get; set; }

        //quan he 
        public Order Order { get; set; }
    }
}
