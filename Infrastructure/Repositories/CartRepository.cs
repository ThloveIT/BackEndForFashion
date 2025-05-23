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
                .Include(c=>c.CartItems)
                .ThenInclude(ci=>ci.Product)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
