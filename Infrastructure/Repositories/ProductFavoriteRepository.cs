using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class ProductFavoriteRepository : Repository<ProductFavorite>, IProductFavorityRepository
    {
        public ProductFavoriteRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<ProductFavorite> GetByUserAndProductAsync(Guid UserId, Guid ProductId)
        {
            return await _context.ProductFavorites
                   .FirstOrDefaultAsync(pf => pf.UserId == UserId && pf.ProductId == ProductId);
        }

        public async Task<IEnumerable<ProductFavorite>> GetByUserIdAsync(Guid UserId)
        {
            return await _context.ProductFavorites
                .Where(pf => pf.UserId == UserId)
                .Include(pf=>pf.Product)
                .ThenInclude(pf => pf.ProductImages)
                .ToListAsync();
        }
    }
}
