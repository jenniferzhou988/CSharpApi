using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ShippingTracking")]
    public class ShippingTracking : GDCTEntityBase<int>
    {
        public int ShippingServiceProviderId { get; set; }

        [ForeignKey(nameof(ShippingServiceProviderId))]
        public ShippingServiceProvider ShippingServiceProvider { get; set; } = null!;

        [Required]
        [MaxLength(200)]
        public string TrackingNumber { get; set; } = null!;

        [MaxLength(1000)]
        public string? Comment { get; set; }

        [Required]
        public DateTime ShippingDate { get; set; }

        public ICollection<ShippingItemDetail> ShippingItemDetails { get; set; } = new List<ShippingItemDetail>();
    }
}