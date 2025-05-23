namespace BackEndForFashion.Application.ViewModels
{
    public class ProductFavariteVM
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string PrimaryImageUrl { get; set; }
    }
}
