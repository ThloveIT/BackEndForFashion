namespace BackEndForFashion.Domain.Entities
{
    public class ProductImage
    {
        public Guid Id { get; set; }
        public string ImageUrl { get; set; }
        // anh chinh hien thi mac dinh
        public bool IsPrimary { get; set; }

        //quan he
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
