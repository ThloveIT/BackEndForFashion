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

        public async Task<Category> GetByIdWithParentAsync(Guid id)
        {
            return await _context.Categories
                .Include(c=>c.Parent)
                .FirstOrDefaultAsync(c=>c.Id == id);
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            return await _context.Categories.Where(c=>c.ParentId == null)
                                            .Include(c=>c.SubCategories)
                                            .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid ParentId)
        {
            var parentExists = await _context.Categories.AnyAsync(c=>c.Id == ParentId);
            if (!parentExists)
            {
                throw new KeyNotFoundException($"Parent category with ID {ParentId} not found.");
            }
            return await _context.Categories
                .Where(c => c.ParentId == ParentId)
                .Include(c => c.Parent)
                .ToListAsync();
        }
    }
}
