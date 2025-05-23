namespace BackEndForFashion.Application.ViewModels
{
    public class CartVM
    {
        public Guid Id { get; set; }
        public decimal TotalPrice { get; set; }
        public List<CartItemVM> Items { get; set; }
    }
}
