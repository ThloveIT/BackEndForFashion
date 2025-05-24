using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IAboutService
    {
        Task<AboutVM> GetLatestAsync();
        Task<AboutVM> CreateAsync(AboutVM model);
    }
}
