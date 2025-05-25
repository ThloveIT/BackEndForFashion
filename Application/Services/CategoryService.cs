using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<CategoryVM> CreateAsync(CategoryVM model)
        {
            //tao moi mot category
            var category = _mapper.Map<Category>(model);
            category.Id = Guid.NewGuid();
            await _categoryRepository.AddAsync(category);
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task DeleteAsync(Guid Id)
        {
            await _categoryRepository.DeleteAsync(Id);
        }

        public async Task<IEnumerable<CategoryVM>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<CategoryVM[]>(categories);
        }

        public async Task<CategoryVM> GetByIdAsync(Guid Id)
        {
            var category = await _categoryRepository.GetByIdAsync(Id);
            if(category == null)
            {
                throw new Exception("Không có danh mục này");
            }
            return _mapper.Map<CategoryVM>(category);
        }

        public async Task<IEnumerable<CategoryVM>> GetRootCategoriesAsync()
        {
            var rootcategories = await _categoryRepository.GetRootCategoriesAsync();
            return _mapper.Map<IEnumerable<CategoryVM>>(rootcategories);
        }

        public async Task<IEnumerable<CategoryVM>> GetSubCategoriesAsync(Guid ParentId)
        {
            var subcategories = await _categoryRepository.GetSubCategoriesAsync(ParentId);
            return _mapper.Map<IEnumerable<CategoryVM>>(subcategories);
        }

        public async Task UpdateAsync(CategoryVM model)
        {
            var category = await _categoryRepository.GetByIdAsync(model.Id);
            if(category == null)
            {
                throw new Exception("Danh mục này không tồn tại");
            }
            _mapper.Map(model, category);
            await _categoryRepository.UpdateAsync(category);
        }
    }
}
