using BackEndForFashion.Application.Interfaces;
using BackEndForFashion.Application.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackEndForFashion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;

        public ArticlesController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        //Lay theo danh muc
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var articles = await _articleService.GetByCategoryAsync(categoryId);
            return Ok(articles);
        }

        //Lay theo id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var article = await _articleService.GetByIdAsync(id);   
                return Ok(article);
            }catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        //Admin tao moi
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]ArticleVM model)
        {
            try
            {
                var article = await _articleService.CreateAsync(model);
                return CreatedAtAction(nameof(GetById), new {id = article.Id }, article);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Sua xoa
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody]ArticleVM model)
        {
            await _articleService.UpdateAsync(id, model);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _articleService.DeleteAsync(id);
            return NoContent();
        }

    }
}
