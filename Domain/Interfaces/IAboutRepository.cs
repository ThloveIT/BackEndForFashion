using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IAboutRepository : IRepository<About>
    {
        Task<About> GetLastestAsync();
    }
}
