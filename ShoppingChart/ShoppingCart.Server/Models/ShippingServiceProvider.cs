using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ShippingServiceProvider")]
    public class ShippingServiceProvider : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string ProviderName { get; set; } = null!;

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public ICollection<ShippingTracking> ShippingTrackings { get; set; } = new List<ShippingTracking>();
    }
}