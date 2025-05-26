using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;

namespace BackEndForFashion.Application.Services
{
    public class AboutService : IAboutService
    {
        private readonly IAboutRepository _aboutRepository;
        private readonly IMapper _mapper;

        public AboutService(IAboutRepository aboutRepository, IMapper mapper)
        {
            _aboutRepository = aboutRepository;
            _mapper = mapper;
        }
        public async Task<AboutVM> CreateAsync(AboutVM model)
        {
            var about = _mapper.Map<About>(model);
            about.Id = Guid.NewGuid();
            about.CreatedAt = DateTime.UtcNow;
            await _aboutRepository.AddAsync(about);
            return _mapper.Map<AboutVM>(model);
        }

        public async Task<AboutVM> GetLatestAsync()
        {
            var about = await _aboutRepository.GetLastestAsync();
            if (about == null) throw new Exception("Bài viết không tìm thấy");
            return _mapper.Map<AboutVM>(about);
        }
    }
}
