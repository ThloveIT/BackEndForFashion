using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IOrderService
    {
        //Lay don hang theo ID
        Task<OrderVM> GetByIdAsync(Guid Id);
        //Lay don hang theo UserID
        Task<IEnumerable< OrderVM>> GetByUserIdAsync(Guid UserId);
        //Tao don hang moi
        Task<OrderVM> CreateAsync(OrderVM model, Guid UserId);
        ////Cap nhat don hang
        //Task UpdateAsync(Guid Id, string Status);
        //User huy don hang
        Task CancelAsync(Guid OrderId, Guid UserId);
        Task<IEnumerable<OrderVM>> GetAllOrderAsync();
        Task<OrderVM> UpdateAsync(Guid id, UpdateOrderStatus model);
    }
}
