using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        Task<IEnumerable<CartItem>> GetByCartIdAsync(Guid CartId);
        Task<CartItem> GetByCartAndProductAsync(Guid CartId, Guid ProductId);
    }
}
