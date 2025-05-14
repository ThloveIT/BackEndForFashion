namespace BackEndForFashion.Entities
{
    public class Product
    {
        public Guid Id { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; } 
        public decimal Price { get; set; }
        //so luong ton kho
        public int Stock {  get; set; }
        //San pham noi bat
        public bool IsFeatured { get; set; }
        //san pham pho bien
        public bool IsPopular { get; set; }
        //san pham moi nhat
        public bool IsNew { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        //quan he
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
        public ICollection<ProductFavorite> ProductFavorites { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
    }
}
