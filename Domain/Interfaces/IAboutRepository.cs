using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IAboutRepository
    {
        Task<About> GetLastestAsync();
    }
}
