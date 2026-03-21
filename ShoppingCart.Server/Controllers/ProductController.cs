using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Models;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repo;

        public ProductController(IProductRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _repo.GetAllAsync();
            return Ok(items);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return NotFound();
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _repo.CreateAsync(product);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            if (id != product.Id) return BadRequest("Id mismatch.");

            var updated = await _repo.UpdateAsync(product);
            if (updated == null) return NotFound();
            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("{productId:int}/images")]
        public async Task<IActionResult> AddImage(int productId, [FromBody] ProductImage image)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            image.ProductId = productId;
            var result = await _repo.AddImageAsync(productId, image);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{productId:int}/images/{imageId:int}")]
        public async Task<IActionResult> RemoveImage(int productId, int imageId)
        {
            var removed = await _repo.RemoveImageAsync(productId, imageId);
            if (!removed) return NotFound();
            return NoContent();
        }

        [HttpPost("{productId:int}/categories/{categoryId:int}")]
        public async Task<IActionResult> AddCategory(int productId, int categoryId)
        {
            var result = await _repo.AddCategoryAsync(productId, categoryId);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{productId:int}/categories/{categoryId:int}")]
        public async Task<IActionResult> RemoveCategory(int productId, int categoryId)
        {
            var removed = await _repo.RemoveCategoryAsync(productId, categoryId);
            if (!removed) return NotFound();
            return NoContent();
        }

        [HttpPost("{productId:int}/import")]
        public async Task<IActionResult> ImportProduct(int productId, [FromBody] ImportProductRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _repo.ImportProductAsync(productId, request.ImportPrice, request.Quantity, request.Comment);
            if (result == null) return NotFound();
            return Ok(result);
        }
    }

    public class ImportProductRequest
    {
        public decimal ImportPrice { get; set; }
        public int Quantity { get; set; }
        public string? Comment { get; set; }
    }
}