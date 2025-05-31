using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetSubCategoriesAsync(Guid ParentId);
        Task<IEnumerable<Category>> GetRootCategoriesAsync();
        Task<Category> GetByIdWithParentAsync(Guid id); 
    }
}
