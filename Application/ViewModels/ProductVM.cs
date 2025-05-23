namespace BackEndForFashion.Application.ViewModels
{
    public class ProductVM
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsFeatured { get; set; }
        public bool IsPopular { get; set; }
        public bool IsNew { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<ProductImageVM> ProductImages { get; set; }
    }
}
