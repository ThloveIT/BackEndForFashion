using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CartService(ICartRepository cartRepository, ICartItemRepository cartItemRepository, IProductRepository productRepository, IMapper mapper)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task AddItemAsync(Guid UserId, CartItemVM item)
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _cartRepository.AddAsync(cart);
            }

            var existingItem = await _cartItemRepository.GetByCartAndProductAsync(cart.Id, item.ProductId);
            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product == null) throw new Exception("Sản phẩm không thấy");

                var cartItem = new CartItem
                {
                    Id= Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                await _cartItemRepository.AddAsync(cartItem);
            }
        }

        public Task<CartVM> GetActiveCartAsync(Guid UserId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemAsync(Guid UserId, Guid ProductId)
        {
            throw new NotImplementedException();
        }
    }
}
