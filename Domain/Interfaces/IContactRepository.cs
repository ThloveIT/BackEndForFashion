using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IContactRepository : IRepository<Contact>
    {
        Task<IEnumerable<Contact>> GetUnresolvedAsync();
    }
}
