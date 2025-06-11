using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using BackEndForFashion.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //Tao moi 1 user - dang ky
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userService.RegisterAsync(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody]RegisterVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var user = await _userService.RegisterAdminAsync(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Dang nhap
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginVM model)
        {
            try
            {
                var token = await _userService.LoginAsync(model);
                return Ok(new {Token = token});
            }catch (Exception ex)
            {
                return BadRequest(new {Message = ex.Message});
            }
        }
        //Lay thong tin nguoi dung
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = await _userService.GetByIdAsync(userId);
            return Ok(user);
        }

        //Lay toan bo danh sach nguoi dung
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        //Xoa nguoi dung 
        [Authorize(Roles ="Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            if(id == userId)
            {
                return BadRequest("Bạn không thể xóa tài khoản của mình");
            }

            var result = await _userService.DeleteUserAsync(id);
            if (!result)
            {
                return NotFound("User không tồn tại hoặc đã bị xóa.");
            }
            return Ok("Bạn đã xóa user thành công");
        }

        //cap nhat thong tin nguoi dung
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserVM model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Lấy userId từ token của người đăng nhập hiện tại
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                if(userId == Guid.Empty)
                {
                    return BadRequest(new { message = "Không tìm thấy thông tin người dùng." });
                }
                model.Id = userId; // Đảm bảo không ai chỉnh sửa ID

                // Gọi service để cập nhật
                var updatedUser = await _userService.UpdateAsync(model);
                if (updatedUser == null)
                {
                    return NotFound(new { message = "Không tìm thấy người dùng để cập nhật." });
                }
                return Ok(new { statusCode = 200, message = "Cập nhật thông tin thành công", data = updatedUser }); // Có thể trả về thông tin mới cập nhật
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }


    }
}
