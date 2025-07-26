using AutoMapper;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserVM>();

            CreateMap<RegisterVM, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore());

            CreateMap<Category, CategoryVM>()
                .ForMember(dest => dest.ParentName, otp => otp.MapFrom(src => src.Parent != null ? src.Parent.CategoryName : null));

            CreateMap<CategoryVM, Category>()
                .ForMember(dest => dest.ParentId, otp => otp.MapFrom(src => src.ParentId))
                .ForMember(dest => dest.Parent, otp => otp.Ignore())
                .ForMember(dest => dest.Products, otp => otp.Ignore())
                .ForMember(dest => dest.SubCategories, otp => otp.Ignore());

            CreateMap<Product, ProductVM>()
                .ForMember(dest => dest.CategoryName, otp => otp.MapFrom
                (src => src.Category.CategoryName));

            CreateMap<ProductVM, Product>()
                .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id == Guid.Empty))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
                .ForMember(dest => dest.ProductImages, opt => opt.MapFrom(src => src.ProductImages.Select(img => new ProductImage
                {
                    Id = img.Id,
                    ImageUrl = img.ImageUrl,
                    IsPrimary = img.IsPrimary,
                    ProductId = src.Id
                })))
                .ReverseMap();

            CreateMap<ProductImageVM, ProductImage>()
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<ProductFavorite, ProductFavariteVM>()
                .ForMember(dest => dest.ProductName, otp => otp.MapFrom
                (src => src.Product.ProductName))
                .ForMember(dest => dest.Price, otp => otp.MapFrom
                (src => src.Product.Price))
                .ForMember(dest => dest.PrimaryImageUrl, otp => otp
                .MapFrom(src => src.Product.ProductImages
                .FirstOrDefault(pi => pi.IsPrimary).ImageUrl));

            CreateMap<Article, ArticleVM>()
                .ForMember(dest => dest.ArticleCategoryName, otp => otp
                .MapFrom(src => src.ArticleCategory.ArticleCategoryName));

            //mới thêm
            CreateMap<ArticleVM, Article>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true))
                .ReverseMap();

            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.Status, otp => otp.
                MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.ContactName, opt => opt.MapFrom(src => src.ContactName))
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhoneNumber, opt => opt.MapFrom(src => src.ContactPhoneNumber));
            CreateMap<OrderVM, Order>()
               .ForMember(dest => dest.Status, opt => opt.Ignore()) // Bo qua Status tu model
                .ForMember(dest => dest.OrderCode, opt => opt.Ignore()) // Bo qua OrderCode tu model
                .ForMember(dest => dest.ContactEmail, opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ContactPhoneNumber, opt => opt.MapFrom(src => src.ContactPhoneNumber))
                .ForMember(dest => dest.ShippingAddress, opt => opt.MapFrom(src => src.ShippingAddress))
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails.Select(od => new OrderDetail
                {
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    UnitPrice = od.UnitPrice
                })));
            CreateMap<OrderVM, Payment>()
                .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
                .ForMember(dest => dest.Amount, opt => opt.Ignore()) // Amount được tính trong OrderService
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => PaymentStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.OrderId, opt => opt.Ignore());
            CreateMap<OrderDetail, OrderDetailVM>()
                .ForMember(dest => dest.ProductName, opt => opt
                .MapFrom(src => src.Product.ProductName));
            CreateMap<OrderDetailVM, OrderDetail>();

            CreateMap<Cart, CartVM>()
                .ForMember(dest => dest.TotalPrice, otp => otp
                .MapFrom(src=>src.CartItems.Sum(ci=>ci.Quantity * ci.Product.Price)))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.CartItems ?? new List<CartItem>())); ;


            CreateMap<CartItem, CartItemVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            //.ForMember(dest => dest.ProductName, otp => otp.
            //MapFrom(src => src.Product.ProductName))
            //.ForMember(dest => dest.Price, otp => otp.
            //MapFrom(src => src.Product.Price))
            //.ForMember(dest => dest.ImageUrl, otp => otp.
            //MapFrom(src => src.Product.ProductImages.FirstOrDefault(pi => pi.IsPrimary).ImageUrl));

            CreateMap<CartItem, CartItemDisplayVM>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrl, otp => otp.
                 MapFrom(src => src.Product.ProductImages.FirstOrDefault(pi => pi.IsPrimary).ImageUrl));

            CreateMap<Payment, PaymentVM>().ReverseMap();
            CreateMap<Contact, ContactVM>();
            CreateMap<About, AboutVM>();

        }
    }
}
