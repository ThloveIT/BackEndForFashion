using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;
using System.Security.Claims;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        //Tao moi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]OrderVM model)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if(string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { Message = "Invalid or missing user ID in token." });
                }
                var createdOrder = await _orderService.CreateAsync(model, userId);
                return CreatedAtAction(nameof(GetById), new {id = createdOrder.Id}, createdOrder );
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Lay theo Id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var order = await _orderService.GetByIdAsync(id);   
                return Ok(order);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //Lay theo User
        [HttpGet]
        public async Task<IActionResult> GetByUser()
        {
            var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var orders = await _orderService.GetByUserIdAsync(userId);
            return Ok(orders);
        }

        //Lay tat ca don hang danh cho admin
        [Authorize(Roles = "Admin")]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllOrders()
        {
            try
            {
                var orders = await _orderService.GetAllOrderAsync();
                return Ok(orders);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return BadRequest(new { Message = ex.Message });
            }
        }

        //Cap nhat don hang danh cho Admin
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderVM model)
        {
            try
            {
                var updatedOrder = await _orderService.UpdateAsync(id, model);
                return Ok(updatedOrder);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

    }
}
