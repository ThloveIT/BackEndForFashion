using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {
        public CartRepository(MyDbContext context) : base(context)
        {
        }

        public Task<Cart> GetActiveCartByUserIdAsync(Guid UserId)
        {
            return _context.Carts
                .Where(c => c.UserId == UserId)
                .Include(c => c.CartItems) // Bao gồm các CartItems
                .ThenInclude(ci => ci.Product) // Bao gồm Product liên quan
                .ThenInclude(p => p.ProductImages) // Bao gồm ProductImages của Product
                .OrderByDescending(c => c.CreatedAt) // Lấy giỏ hàng mới nhất
                .FirstOrDefaultAsync();
        }
    }
}
