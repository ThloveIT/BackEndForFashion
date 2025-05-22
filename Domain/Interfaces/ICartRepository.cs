using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<Cart> GetActiveCartByUserIdAsync(Guid UserId);
    }
}
