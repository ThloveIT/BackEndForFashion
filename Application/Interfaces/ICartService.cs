using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface ICartService
    {
        //Lay gio hang theo userId
        Task<CartVM> GetActiveCartAsync(Guid UserId);
        // Them san pham vao gio hang
        Task AddItemAsync(Guid UserId, CartItemVM item);
        //Xoa san pham khoi gio hang
        Task RemoveItemAsync(Guid UserId, Guid ProductId);
    }
}
