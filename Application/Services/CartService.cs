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
            //Kiem tra xem nguoi dung co gio hang chua
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(UserId);
            if (cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    CreatedAt = DateTime.UtcNow,
                };
                // Them gio hang moi vao CSDL
                await _cartRepository.AddAsync(cart);
            }

            //Kiem tra xem san pham exisgingItem co trong Item khong
            var exisgingItem = await _cartItemRepository.GetByCartAndProductAsync(cart.Id, item.ProductId);
            //neu san pham co trong gio hang
            if(exisgingItem !=  null)
            {
                //tang so luong san pham
                exisgingItem.Quantity += item.Quantity;
                //Cap nhat lai item
                await _cartItemRepository.UpdateAsync(exisgingItem);
            }
            //Neu san pham khong co trong gio hang
            else
            {
                //kiem tra xem co trong csdl khong
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if(product == null)
                {
                    throw new Exception("San pham nay khong ton tai");
                }
                // tao item moi
                var cartItem = new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = cart.Id,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                };
                //them cartItem vao CSDL
                await _cartItemRepository.AddAsync(cartItem);
            }
        }

        //Tao gio hang cho user
        public async Task<CartVM> GetActiveCartAsync(Guid UserId)
        {
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(UserId);
            if(cart == null)
            {
                cart = new Cart
                {
                    Id = Guid.NewGuid(),
                    UserId = UserId,
                    CreatedAt = DateTime.UtcNow,
                };
                await _cartRepository.AddAsync(cart);
            }
            return _mapper.Map<CartVM>(cart);
        }

        public async Task RemoveItemAsync(Guid UserId, Guid ProductId)
        {
            // Kiem tra co gio hang khong
            var cart = await _cartRepository.GetActiveCartByUserIdAsync(UserId);
            if (cart == null) return;
            //Kiem tra item co trong gio hang khong
            var item = await _cartItemRepository.GetByCartAndProductAsync(cart.Id, ProductId);
            if(item != null)
            {
                await _cartItemRepository.DeleteAsync(item.Id);
            }
        }
    }
}
