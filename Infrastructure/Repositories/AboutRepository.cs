using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class AboutRepository : Repository<About>, IAboutRepository
    {
        public AboutRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<About> GetLastestAsync()
        {
            return await _context.Abouts
                .OrderByDescending(a=>a.CreatedAt)
                .FirstOrDefaultAsync();
        }
    }
}
