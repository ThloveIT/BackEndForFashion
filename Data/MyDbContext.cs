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

            //Category
            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.CategoryName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e=>e.IsActive).IsRequired().HasDefaultValue(true);
                entity.HasIndex(e=>e.CategoryName).IsUnique();
                //quan he
                entity.HasOne(e => e.Parent)
                .WithMany(e=>e.SubCategories)
                .HasForeignKey(e=>e.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
            });
            //Product
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.ProductName).IsRequired().HasMaxLength(200);
                entity.Property(e=>e.Description).HasMaxLength(1000);
                entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.Stock).IsRequired();
                entity.Property(e=>e.IsFeatured).IsRequired().HasDefaultValue(false);
                entity.Property(e=>e.IsPopular).IsRequired().HasDefaultValue(false);
                entity.Property(e=>e.IsNew).IsRequired().HasDefaultValue(false);
                entity.Property(e=>e.CreatedAt).IsRequired();
                entity.Property(e=>e.IsActive).IsRequired().HasDefaultValue(true);
                //tim kiem
                entity.HasIndex(e => e.ProductName).IsUnique();
                //quan he
                entity.HasOne(e=>e.Category)
                .WithMany(e=>e.Products)
                .HasForeignKey(e=>e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict); 
            });

            //producImage
            modelBuilder.Entity<ProductImage>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ImageUrl).IsRequired().HasMaxLength(500);
                entity.Property(e => e.IsPrimary).IsRequired().HasDefaultValue(false);
                //quan he
                entity.HasOne(e => e.Product)
                .WithMany(e => e.ProductImages)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });
            //productFavorite
            modelBuilder.Entity<ProductFavorite>(entity =>
            {
                entity.HasKey(e=>new {e.UserId, e.ProductId});
                entity.Property(e=>e.AddedAt).IsRequired();
                //quan he
                entity.HasOne(e => e.User)
                .WithMany(e => e.ProductFavorites)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e=>e.Product)
                .WithMany(e=>e.ProductFavorites)
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //Article: Bai viet
            modelBuilder.Entity<Article>(entiy =>
            {
                entiy.HasKey(e => e.Id);
                entiy.Property(e=>e.Title).IsRequired().HasMaxLength(200);
                entiy.Property(e => e.Content).IsRequired();
                entiy.Property(e=>e.ThumbnailUrl).HasMaxLength(500);
                entiy.Property(e=>e.CreatedAt).IsRequired();
                entiy.Property(e=>e.IsActive).IsRequired().HasDefaultValue(true);
                //quan he
                entiy.HasOne(e=>e.ArticleCategory)
                .WithMany(e=>e.Articles)
                .HasForeignKey(e=>e.ArticleCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //Danh muc bai viet: ArticleCategory
            modelBuilder.Entity<ArticleCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.ArticleCategoryName).IsRequired().HasMaxLength(200);
                entity.Property(e=>e.Description).HasMaxLength(500);
                entity.Property(e=>e.IsActive).IsRequired().HasDefaultValue(true);
            });

            //Don hang: Order
            modelBuilder.Entity<Order>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.OrderCode).IsRequired().HasMaxLength(200);
                entity.Property(e => e.TotalAmount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e=>e.ShippingAddress).IsRequired().HasMaxLength(500);
                entity.Property(e=>e.Status).IsRequired()
                .HasConversion<int>()
                .HasDefaultValue(Status.Confirmed);
                entity.Property(e => e.CreatedAt).IsRequired();
                //quan he
                entity.HasOne(e => e.User)
                .WithMany(e => e.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //Chi tiet don hang: OrderDetail
            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.Quantity).IsRequired().HasDefaultValue(1);
                entity.Property(e => e.UnitPrice).IsRequired().HasColumnType("decimal(18,2)");
                //quan he
                entity.HasOne(e => e.Order)
                .WithMany(e=>e.OrderDetails)
                .HasForeignKey(e=>e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e=>e.Product)
                .WithMany(e=>e.OrderDetails)
                .HasForeignKey(e=>e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //Gio hang: Cart
            modelBuilder.Entity<Cart>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.CreatedAt).IsRequired();
                //quan he
                entity.HasOne(e=>e.User)
                .WithMany(e=>e.Carts)
                .HasForeignKey(e=>e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            });
            //muc trong gio hang
            modelBuilder.Entity<CartItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).IsRequired();
                //quan he
                entity.HasOne(e=>e.Cart)
                .WithMany(e=>e.CartItems)
                .HasForeignKey(e=>e.CartId)
                .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e=>e.Product)
                .WithMany(e=>e.CartItems)
                .HasForeignKey(e=>e.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            //thanh toan: payment
            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e=>e.Amount).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(e => e.PaymentMethod).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Status).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).IsRequired();
                //quan he
                entity.HasOne(e=>e.Order)
                .WithOne(e=>e.Payment)
                .HasForeignKey<Payment>(e=>e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            //contact
            modelBuilder.Entity<Contact>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.Message).IsRequired().HasMaxLength(1000);
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.IsResolved).IsRequired().HasDefaultValue(false);
            });
            //about
            modelBuilder.Entity<About>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Content).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
            });
        }
    }
}
