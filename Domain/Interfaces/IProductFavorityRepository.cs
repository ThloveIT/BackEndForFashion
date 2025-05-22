using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IProductFavorityRepository : IRepository<ProductFavorite>
    {
        Task<IEnumerable<ProductFavorite>> GetByUserIdAsync(Guid UserId);
        Task<ProductFavorite> GetByUserAndProductAsync(Guid UserId, Guid ProductId);
    }
}
