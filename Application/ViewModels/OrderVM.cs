namespace BackEndForFashion.Application.ViewModels
{
    public class OrderVM
    {
        public Guid Id { get; set; }
        public string OrderCode { get; set; }
        public decimal TotalAmount { get; set; }
        public string ShippingAddress { get; set; }
        public string Status { get; set; }
        public Guid UserId { get; set; }
        public List<OrderDetailVM> OrderDetails { get; set; }
    }
}
