using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Infrastructure.Repositories
{
    public class ContactRepository : Repository<Contact>, IContactRepository
    {
        public ContactRepository(MyDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Contact>> GetUnresolvedAsync()
        {
            return await _context.Contacts
                .Where(c=> !c.IsResolved)
                .ToListAsync();
        }
    }
}
