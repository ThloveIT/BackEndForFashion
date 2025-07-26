namespace BackEndForFashion.Application.ViewModels
{
    public class CartVM
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItemDisplayVM> Items { get; set; }
    }
}
