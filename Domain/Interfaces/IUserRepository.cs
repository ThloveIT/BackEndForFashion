using BackEndForFashion.Domain.Entities;

namespace BackEndForFashion.Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetByUsernameAsync(string UserName);
        Task<User> GetByEmailAsync(string Email);
        Task<IEnumerable<User>> GetUsersByRoleAsync(string Role);

    }
}
