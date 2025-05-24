using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IProductService
    {
        //1. Lay san pham theo danh muc
        Task<IEnumerable<ProductVM>> GetByCategoryAsync(Guid CategoryId);
        //2. Tim san pham theo tu khoa
        Task<IEnumerable<ProductVM>> SearchAsync(string keyword);
        //3. Lay mot san pham theo id
        Task<ProductVM> GetByIdAsync(Guid Id);
        //4. Lay tat ca san pham
        Task<IEnumerable<ProductVM>> GetAllAsync();
        //5. Them san pham
        Task<ProductVM> CreateAsync(ProductVM model);
        //6. Sua san pham
        Task UpdateAsync(Guid Id, ProductVM model);
        //7. Xoa san pham
        Task DeleteAsync(Guid Id);
        //8. Lay san pham noi bat
        Task<IEnumerable<ProductVM>> GetFeaturedAsync();
        //9. Lay san pham moi nhat
        Task<IEnumerable<ProductVM>> GetNewAsync();
        //10. Lay san pham pho bien
        Task<IEnumerable<ProductVM>> GetPopularAsync();
    }
}
