using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AboutsController : ControllerBase
    {
        private IAboutService _aboutService;

        public AboutsController(IAboutService aboutService)
        {
            _aboutService = aboutService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLastest()
        {
            try
            {
                var about = await _aboutService.GetLatestAsync();
                return Ok(about);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]AboutVM model)
        {
            var about = await _aboutService.CreateAsync(model);
            return CreatedAtAction(nameof(GetLastest), new { id = about.Id }, about);
        }
    }
}
