using BackEndForFashion.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class FavoritesController : ControllerBase
    {
        private readonly IProductFavoriteService _favoriteRepository;

        public FavoritesController(IProductFavoriteService favoriteService)
        {
            _favoriteRepository = favoriteService;
        }

        //Lay danh sach yeu thich
        [HttpGet]
        public async Task<IActionResult> GetFavorites()
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var favorites = await _favoriteRepository.GetByUserIdAsync(userId);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Them vao danh sach yeu thich
        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(Guid productId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _favoriteRepository.AddFavoriteAsync(userId, productId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message); 
            }
        }
        //Xoa khoi danh sach
        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFavorite(Guid productId)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _favoriteRepository.RemoveFavoriteAsync(userId, productId);
                return NoContent();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
