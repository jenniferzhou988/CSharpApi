using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Repository.Interface
{
    public interface IShippingTrackingRepository : IGDCTRepository<ShippingTracking>
    {
        Task<ShippingTracking?> CreateShippingTrackingAsync(CreateShippingTrackingRequest request);
        Task<ShippingTracking?> GetByIdAsync(int id);
        Task<IEnumerable<ShippingTracking>> GetAllAsync();
    }

    public class CreateShippingTrackingRequest
    {
        public int OrderId { get; set; }
        public string ProviderName { get; set; } = null!;
        public string TrackingNumber { get; set; } = null!;
        public DateTime ShippingDate { get; set; }
        public string? Comment { get; set; }
        public List<int> OrderDetailIds { get; set; } = new();
    }
}