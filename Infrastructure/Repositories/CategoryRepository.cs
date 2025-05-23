using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            return await _context.Categories.Where(c=>c.ParentId == null)
                                            .Include(c=>c.SubCategories)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid ParentId)
        {
            return await _context.Categories
                .Where(c => c.ParentId == ParentId)
                .Include(c => c.Parent)
                .ToListAsync();
        }
    }
}
