using AutoMapper;
using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Domain.Entities;
using BackEndForFashion.Domain.Interfaces;
using BackEndForFashion.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;

namespace BackEndForFashion.Application.Services
{
    public class UserService : IUserService
    {
        private IUserRepository _userRepository;
        private IJwtService _jwtService;
        private IMapper _mapper;
        private IPasswordHasher<User> _passwordHasher;

        public UserService(IUserRepository userRepository, IJwtService jwtService, IMapper mapper, IPasswordHasher<User> passwordHasher) 
        {
            _userRepository = userRepository;
            _jwtService = jwtService;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
        }

        public async Task<bool> DeleteUserAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            if (user == null) return false;
            await _userRepository.DeleteAsync(Id);
            return true;
        }

        public async Task<IEnumerable<UserVM>> GetAllAsync()
        {
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserVM>>(users);
        }

        public async Task<UserVM> GetByIdAsync(Guid Id)
        {
            var user = await _userRepository.GetByIdAsync(Id);
            if(user == null)
            {
                throw new Exception("Người dùng không tồn tại");
            }
            return  _mapper.Map<UserVM>(user);
        }

        public async Task<string> LoginAsync(LoginVM model)
        {
            //tim user cua model theo username
            var user = await _userRepository.GetByUsernameAsync(model.UserName);
            if(user == null)
            {
                throw new Exception("Sai tài khoản hoặc mật khẩu");
            }
            else
            {
                //so sanh mat khau cua user trong CSDL va nguoi dung nhap
                var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
                if(result != PasswordVerificationResult.Success)
                {
                    throw new Exception("Sai tài khoản hoặc mật khẩu");
                }
            }
            return _jwtService.GenerateToken(user);
        }

        public async Task<UserVM> RegisterAdminAsync(RegisterVM model)
        {
            return await RegisterAsync(model, "Admin");
        }

        public async Task<UserVM> RegisterAsync(RegisterVM model, string Role = "User")
        {
            //ktra username va email co trung khong
            if(await _userRepository.GetByEmailAsync(model.Email) != null)
            {
                throw new Exception("Email này đã tồn tại");
            }
            if(await _userRepository.GetByUsernameAsync(model.UserName) != null)
            {
                throw new Exception("User này đã tồn tại");
            }
            //Tao user moi
            var user = _mapper.Map<User>(model);
            user.Id = Guid.NewGuid();
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            user.IsActive = true;
            user.Role = Role;
            user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserVM>(user);
        }
    }
}
