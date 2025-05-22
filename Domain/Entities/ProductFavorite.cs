namespace BackEndForFashion.Domain.Entities
{
    public class ProductFavorite
    {

        // thoi gian them san pham vao danh sach yeu thich
        public DateTime AddedAt { get; set; }

        // quan he trung gian cho n-n user va product
        public Guid UserId { get; set; }
        public User User { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }

    }
}
