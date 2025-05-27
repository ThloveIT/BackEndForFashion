using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
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
            try
            {
                var user = await _userService.RegisterAsync(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
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
    }
}
