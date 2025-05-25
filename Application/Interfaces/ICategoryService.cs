using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryVM>> GetAllAsync();
        Task<CategoryVM> GetByIdAsync(Guid Id);
        Task<IEnumerable<CategoryVM>> GetSubCategoriesAsync(Guid ParentId);
        Task<IEnumerable<CategoryVM>> GetRootCategoriesAsync();

        //admin only
        Task<CategoryVM> CreateAsync(CategoryVM model);
        Task UpdateAsync(CategoryVM model);
        Task DeleteAsync(Guid Id);
    }
}
