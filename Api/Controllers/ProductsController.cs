using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }
        //Lay danh sach san pham
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        //Lay san pham theo ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid Id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(Id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Tim kiem san pham
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string keyword)
        {
            var products = await _productService.SearchAsync(keyword);
            return Ok(products);
        }

        //Lay theo danh muc san pham
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productService.GetByCategoryAsync(categoryId);
            return Ok(products);
        }

        //Lay danh sach san pham noi bat
        [HttpGet("featured")]
        public async Task<IActionResult> GetFeatured()
        {
            var products = await _productService.GetFeaturedAsync();
            return Ok(products);
        }

        //Lay danh sach san pham pho bien nhat
        [HttpGet("popular")]
        public async Task<IActionResult> GetPopular()
        {
            var products = await _productService.GetPopularAsync();
            return Ok(products);
        }

        //Lay danh sach san pham moi nhat
        [HttpGet("new")]
        public async Task<IActionResult> GetNew()
        {
            var products = await _productService.GetNewAsync();
            return Ok(products);
        }

        //Admin tao san pham moi
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductVM model)
        {
            var product = await _productService.CreateAsync(model);
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }
        //Admin cap nhat san pham
        [Authorize(Roles = "Admin")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]ProductVM model)
        {
            await _productService.UpdateAsync(id, model);
            return NoContent();
        }

        //Admin xoa san pham
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _productService.DeleteAsync(id);
            return NoContent();
        }
    }
}
