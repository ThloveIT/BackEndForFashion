using BackEndForFashion.Application.ViewModels;

namespace BackEndForFashion.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserVM> RegisterAsync(RegisterVM model, string Role = "User");
        Task<UserVM> RegisterAdminAsync(RegisterVM model);

        //truyen vao loginVM tra ve JW token
        Task<string> LoginAsync(LoginVM model);
        Task<UserVM> GetByIdAsync(Guid Id);
        Task<IEnumerable<UserVM>> GetAllAsync(); //Admin only
        Task<bool> DeleteUserAsync(Guid Id);
        Task<UserVM> UpdateAsync(UpdateUserVM model);
    }
}
