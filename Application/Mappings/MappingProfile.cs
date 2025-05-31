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

            CreateMap<Order, OrderVM>()
                .ForMember(dest => dest.Status, otp => otp.
                MapFrom(src => src.Status.ToString()));
            CreateMap<OrderVM, Order>();
            CreateMap<OrderDetail, OrderDetailVM>()
                .ForMember(dest => dest.ProductName, opt => opt
                .MapFrom(src => src.Product.ProductName)); ;

            CreateMap<Cart, CartVM>()
                .ForMember(dest => dest.TotalPrice, otp => otp
                .MapFrom(src=>src.CartItems.Sum(ci=>ci.Quantity * ci.Product.Price)));

            CreateMap<CartItem, CartItemVM>()
                .ForMember(dest => dest.ProductName, otp => otp.
                MapFrom(src => src.Product.ProductName))
                .ForMember(dest => dest.Price, otp => otp.
                MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.ImageUrl, otp => otp.
                MapFrom(src => src.Product.ProductImages.FirstOrDefault(pi => pi.IsPrimary).ImageUrl));

            CreateMap<Payment, PaymentVM>();
            CreateMap<Contact, ContactVM>();
            CreateMap<About, AboutVM>();

        }
    }
}
