using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;

        public ContactsController(IContactService contactService)
        {
            _contactService = contactService;
        }

        //Tao moi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ContactVM model)
        {
            try
            {
                var createContact = await _contactService.CreateAsync(model);
                return CreatedAtAction(nameof(Create), new { id = createContact.Id }, createContact);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Danh sach chua giai quyet
        [Authorize(Roles ="Admin")]
        [HttpGet("unresolved")]
        public async Task<IActionResult> GetUnResolved()
        {
            var contacts = await _contactService.GetUnresolvedAsync(); 
            return Ok(contacts);
        }

        //Giai quyet
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}/resolve")]
        public async Task<IActionResult> Resolve(Guid id)
        {
            await _contactService.ResolveAsync(id);
            return NoContent();
        }
    }
}
