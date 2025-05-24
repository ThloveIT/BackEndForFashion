using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IArticleService
    {
        //Lay danh sach tin tuc
        Task<IEnumerable<ArticleVM>> GetAllAsync();
        //Lay danh sach tin tuc theo danh muc
        Task<IEnumerable<ArticleVM>> GetByCategoryAsync(Guid ArticleCategoryId);
        //Lay tin tuc theo id
        Task<ArticleVM> GetByIdAsync(Guid Id);
        //Them tin tuc moi
        Task<ArticleVM> CreateAsync(ArticleVM model);
        //Cap nhat tin tuc
        Task UpdateAsync(Guid Id, ArticleVM model);
        //Xoa tin tuc
        Task DeleteAsync(Guid Id);
    }
}
