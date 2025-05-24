using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IProductFavoriteService
    {
        //Lay danh sach san pham yeu thich theo userID
        Task<IEnumerable<ProductFavariteVM>> GetByUserIdAsync(Guid UserId);
        //Them san pham yeu thich
        Task AddFavoriteAsync(Guid UserId, Guid ProductId);
        //Xoa san pham yeu thich
        Task RemoveFavoriteAsync(Guid UserId, Guid ProductId);
    }
}

