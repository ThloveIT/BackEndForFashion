using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByUserIdAsync(Guid UserId);
        Task<Order> GetByUserIdAndOrderIdAsync(Guid UserId, Guid OrderId);
    }
}
