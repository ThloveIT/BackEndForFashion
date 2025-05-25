using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BackEndForFashion.Application.Services
{
    public class ProductService : IProductService
    {
        
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ProductVM> CreateAsync(ProductVM model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }
            var product = _mapper.Map<Product>(model);
            product.Id = Guid.NewGuid();
            product.CreatedAt = DateTime.UtcNow;
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.AddAsync(product);
            return _mapper.Map<ProductVM>(product);
        }

        public async Task DeleteAsync(Guid Id)
        {
            await _productRepository.DeleteAsync(Id);
        }

        public async Task<IEnumerable<ProductVM>> GetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> GetByCategoryAsync(Guid CategoryId)
        {
            var products = await _productRepository.GetByCategoryAsync(CategoryId);
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<ProductVM> GetByIdAsync(Guid Id)
        {
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null) throw new ArgumentNullException("Không tìm thấy sản phẩm");
            return _mapper.Map<ProductVM>(product);
        }

        public async Task<IEnumerable<ProductVM>> GetFeaturedAsync()
        {
            var products = await _productRepository.GetFeaturedAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> GetNewAsync()
        {
            var products = await _productRepository.GetNewAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> GetPopularAsync()
        {
            var products = await _productRepository.GetPopularAsync();
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task<IEnumerable<ProductVM>> SearchAsync(string keyword)
        {
            var products = await _productRepository.SearchAsync(keyword);
            return _mapper.Map<IEnumerable<ProductVM>>(products);
        }

        public async Task UpdateAsync(Guid Id, ProductVM model)
        {
            var product = await _productRepository.GetByIdAsync(Id);
            if (product == null)
            {
                throw new Exception("Không tìm thấy sản phẩm");
            }
            //chuyen du lieu nguoi dung nhap model sang du lieu product
            _mapper.Map(model, product);
            product.UpdatedAt = DateTime.UtcNow;
            await _productRepository.UpdateAsync(product);
        }
    }
}
