using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<CartItem> GetByCartAndProductAsync(Guid CartId, Guid ProductId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.CartId == CartId && ci.ProductId == ProductId);
        }

        public async Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid CartId)
        {
            return await _context.CartItems
                .Where(ci => ci.CartId == CartId)
                .Include(ci => ci.Product)
                .ToListAsync();
        }
    }
}
