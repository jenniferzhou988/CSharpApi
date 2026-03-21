using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ShippingItemDetail")]
    public class ShippingItemDetail : GDCTEntityBase<int>
    {
        public int ShippingTrackingId { get; set; }

        [ForeignKey(nameof(ShippingTrackingId))]
        public ShippingTracking ShippingTracking { get; set; } = null!;

        public int OrderDetailId { get; set; }

        [ForeignKey(nameof(OrderDetailId))]
        public OrderDetail OrderDetail { get; set; } = null!;
    }
}