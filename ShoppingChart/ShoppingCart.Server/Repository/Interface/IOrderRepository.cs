using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IOrderRepository : IGDCTRepository<Order>
    {
        Task<Order?> CreateOrderAsync(int customerId, int bankCardId, int shippingAddressId, int billingAddressId, List<CreateOrderItemRequest> items);
        Task<Order?> GetByIdAsync(int id);
        Task<IEnumerable<Order>> GetAllAsync();
    }

    public class CreateOrderItemRequest
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? Comment { get; set; }
    }
}