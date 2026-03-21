using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartRepository _repo;

        public ShoppingCartController(IShoppingCartRepository repo)
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
        public async Task<IActionResult> Create([FromBody] CreateShoppingCartRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _repo.CreateAsync(request.CustomerId);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _repo.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        [HttpPost("{shoppingCartId:int}/items")]
        public async Task<IActionResult> AddItem(int shoppingCartId, [FromBody] AddItemRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _repo.AddItemAsync(shoppingCartId, request.ProductId, request.Quantity);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpPut("{shoppingCartId:int}/items/{detailId:int}")]
        public async Task<IActionResult> UpdateItem(int shoppingCartId, int detailId, [FromBody] UpdateItemRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _repo.UpdateItemAsync(shoppingCartId, detailId, request.Quantity);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{shoppingCartId:int}/items/{detailId:int}")]
        public async Task<IActionResult> RemoveItem(int shoppingCartId, int detailId)
        {
            var removed = await _repo.RemoveItemAsync(shoppingCartId, detailId);
            if (!removed) return NotFound();
            return NoContent();
        }
    }

    public class CreateShoppingCartRequest
    {
        public int CustomerId { get; set; }
    }

    public class AddItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }

    public class UpdateItemRequest
    {
        public int Quantity { get; set; }
    }
}