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
    [Authorize(Roles = "User")]
    public class CartsController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartsController(ICartService cartService)
        {
            _cartService = cartService;
        }
        //Lay gio hang
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var cart = await _cartService.GetActiveCartAsync(userId);
            return Ok(cart);
        }

        // Them Item
        [HttpPost("item")]
        public async Task<IActionResult> AddItem([FromBody]CartItemVM item)
        {
            try
            {
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                await _cartService.AddItemAsync(userId, item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Xoa item
        [HttpDelete("item/{productId}")]
        public async Task<IActionResult> RemoveItem(Guid productId)
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _cartService.RemoveItemAsync(userId, productId);
            return NoContent();
        }

    }
}
