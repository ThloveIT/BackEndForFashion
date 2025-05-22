using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> SearchAsync(string keyword);
        Task<IEnumerable<Product>> GetByCategoryAsync(Guid CategoryId);
        Task<IEnumerable<Product>> GetFeaturedAsync();
        Task<IEnumerable<Product>> GetPopularAsync();
        Task<IEnumerable<Product>> GetNewAsync();
    }
}
