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
    [Authorize(Roles = "User")]
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
                var userId = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
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

    }
}
