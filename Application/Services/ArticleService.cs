using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class ArticleService : IArticleService
    {
        private IArticleRepository _articleRepository;
        private IMapper _mapper;

        public ArticleService(IArticleRepository articleRepository, IMapper mapper)
        {
            _articleRepository = articleRepository;
            _mapper = mapper;
        }
        public async Task<ArticleVM> CreateAsync(ArticleVM model)
        {
            var article = _mapper.Map<Article>(model);
            article.Id = Guid.NewGuid();
            article.CreatedAt = DateTime.UtcNow;
            await _articleRepository.AddAsync(article);
            return _mapper.Map<ArticleVM>(article);
        }

        public async Task DeleteAsync(Guid Id)
        {
            await _articleRepository.DeleteAsync(Id);
        }

        public async Task<IEnumerable<ArticleVM>> GetAllAsync()
        {
            var articles = await _articleRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ArticleVM>>(articles);
        }

        public async Task<IEnumerable<ArticleVM>> GetByCategoryAsync(Guid ArticleCategoryId)
        {
            var articles = await _articleRepository.GetByCategoryAsync(ArticleCategoryId);
            return _mapper.Map<IEnumerable<ArticleVM>>(articles);
        }

        public async Task<ArticleVM> GetByIdAsync(Guid Id)
        {
            var article = await _articleRepository.GetByIdAsync(Id);
            if (article == null) throw new Exception("Bài viết không tồn tại");
            return _mapper.Map<ArticleVM>(article);
        }

        public async Task UpdateAsync(Guid Id, ArticleVM model)
        {
            var exising = await _articleRepository.GetByIdAsync(Id);
            if (exising == null) throw new Exception("Bài viết không tồn tại");
            _mapper.Map(model, exising);
            exising.UpdatedAt = DateTime.UtcNow;
            await _articleRepository.UpdateAsync(exising);
        }
    }
}
