using Microsoft.AspNetCore.Mvc;
using ShoppingCartAPI.Repository.Interface;

namespace ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _repo;

        public OrderController(IOrderRepository repo)
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
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (request.Items == null || request.Items.Count == 0)
                return BadRequest("Order must contain at least one item.");

            var order = await _repo.CreateOrderAsync(
                request.CustomerId,
                request.BankCardId,
                request.ShippingAddressId,
                request.BillingAddressId,
                request.Items);

            if (order == null) return NotFound("One or more referenced entities not found.");
            return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }
    }

    public class CreateOrderRequest
    {
        public int CustomerId { get; set; }
        public int BankCardId { get; set; }
        public int ShippingAddressId { get; set; }
        public int BillingAddressId { get; set; }
        public List<CreateOrderItemRequest> Items { get; set; } = new();
    }
}