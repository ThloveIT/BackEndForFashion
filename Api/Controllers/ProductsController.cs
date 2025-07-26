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
        private readonly IWebHostEnvironment _enviroment;

        public ProductsController(IProductService productService, IWebHostEnvironment environment)
        {
            _productService = productService;
            _enviroment = environment;
        }
        //Lay danh sach san pham
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                Console.WriteLine($"Fetching product with ID: {id}");
                var product = await _productService.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound("Không tìm thấy sản phẩm");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetById: {ex.Message} - {ex.StackTrace}");
                return BadRequest(ex.Message);
            }
        }

        //Tim kiem san pham
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var products = await _productService.SearchAsync(query);
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
        public async Task<IActionResult> Create([FromForm] ProductVM model, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                if(images != null && images.Any())
                {
                    model.ProductImages ??= new List<ProductImageVM>();
                    foreach(var image in images)
                    {
                        var imageUrl = await SaveImage(image);
                        model.ProductImages.Add(new ProductImageVM
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = imageUrl,
                            IsPrimary = model.ProductImages.Count == 0
                        });
                    }
                }
                else
                {
                    model.ProductImages = new List<ProductImageVM>();
                }

                model.CreatedAt = DateTime.UtcNow;
                model.IsActive = true;

                var product = await _productService.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        private async Task<string> SaveImage(IFormFile file)
        {
            var uploadsFolder = Path.Combine(_enviroment.WebRootPath, "images/products");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                return $"/images/products/{fileName}";
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error saving image: {ex.Message}");
                throw;
            }
        }

        //Admin cap nhat san pham
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm]ProductVM model, List<IFormFile> images)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                if (images != null && images.Any())
                {
                    model.ProductImages = model.ProductImages ?? new List<ProductImageVM>();
                    foreach (var image in images)
                    {
                        var imageUrl = await SaveImage(image);
                        model.ProductImages.Add(new ProductImageVM
                        {
                            Id = Guid.NewGuid(),
                            ImageUrl = imageUrl,
                            IsPrimary = !model.ProductImages.Any(i => i.IsPrimary), // Đặt ảnh mới làm chính nếu chưa có
                        });
                    }
                }

                model.UpdatedAt = DateTime.UtcNow;
                await _productService.UpdateAsync(id, model);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Admin xoa san pham
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                await _productService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
