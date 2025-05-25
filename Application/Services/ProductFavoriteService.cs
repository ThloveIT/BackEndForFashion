using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class ProductFavoriteService : IProductFavoriteService
    {
        private readonly IProductFavorityRepository _productFavoriteRepository;
        private readonly IMapper _mapper;

        public ProductFavoriteService(IProductFavorityRepository productFavoriteRepository, IMapper mapper)
        {
            _productFavoriteRepository = productFavoriteRepository;
            _mapper = mapper;
        }
        public async Task AddFavoriteAsync(Guid UserId, Guid ProductId)
        {
             var existing = await _productFavoriteRepository.GetByUserAndProductAsync(UserId, ProductId);
             if (existing != null) return;
            var favorite = new ProductFavorite 
            {
                UserId = UserId,
                ProductId = ProductId,
                AddedAt = DateTime.UtcNow,
            };
            await _productFavoriteRepository.AddAsync(favorite);
        }

        public async Task<IEnumerable<ProductFavariteVM>> GetByUserIdAsync(Guid UserId)
        {
            var products = await _productFavoriteRepository.GetByUserIdAsync(UserId);
            return _mapper.Map<IEnumerable<ProductFavariteVM>>(products);
        }

        public async Task RemoveFavoriteAsync(Guid UserId, Guid ProductId)
        {
            var product = await _productFavoriteRepository.GetByUserAndProductAsync(UserId, ProductId);
            await _productFavoriteRepository.DeleteAsync(product.ProductId);
        }
    }
}
