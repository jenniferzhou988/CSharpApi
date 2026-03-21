using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShippingTrackingController : ControllerBase
    {
        private readonly IShippingTrackingRepository _repo;

        public ShippingTrackingController(IShippingTrackingRepository repo)
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
        public async Task<IActionResult> Create([FromBody] CreateShippingTrackingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.OrderDetailIds == null || request.OrderDetailIds.Count == 0)
                return BadRequest("At least one OrderDetailId is required.");

            var result = await _repo.CreateShippingTrackingAsync(request);
            if (result == null) return NotFound("Order or referenced entities not found.");
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }
    }
}