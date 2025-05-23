using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class ArticleRepository : Repository<Article>, IArticleRepository
    {
        public ArticleRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Article>> GetByCategoryAsync(Guid CategoryId)
        {
            return await _context.Articles
                .Where(a=>a.ArticleCategoryId == CategoryId)
                .Include(a => a.ArticleCategory)
                .ToListAsync();
        }
    }
}
