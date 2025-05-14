using BackEndForFashion.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Data
{
    public class MyDbContext : DbContext
    {
        // khoi tao constructor cua MyDbContext
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        //Tao DbSet: Cac bang trong co so du lieu
        public DbSet<User> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<ProductFavorite> ProductFavorites { get; set; }
        public DbSet<Article> Articles { get; set; }    
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<About> Abouts { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        //ghi de cac cau hinh vao lop OnModeCreating
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.UserName).IsRequired().HasMaxLength(50);
                entity.Property(e=>e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e=>e.PasswordHash).IsRequired();
                entity.Property(e=>e.FullName).IsRequired().HasMaxLength(100);
                entity.Property(e=>e.PhoneNumber).IsRequired().HasMaxLength(20);
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e=>e.IsActive).IsRequired().HasDefaultValue(true);
                //truong hop tim kiem nhanh
                entity.HasIndex(e=> e.Email).IsUnique();
                entity.HasIndex(e=>e.UserName).IsUnique();
            });
        }
    }
}
