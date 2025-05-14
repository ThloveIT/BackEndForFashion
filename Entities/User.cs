namespace BackEndForFashion.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }    
        public string Email { get; set; }
        public string PasswordHash { get; set; }
       
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }


        //Quan hệ
        public ICollection<ProductFavorite> ProductFavorites { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<Cart> Carts { get; set; }

    }
}