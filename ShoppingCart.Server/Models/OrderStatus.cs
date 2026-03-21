using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("OrderStatus")]
    public class OrderStatus : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string OrderStatusName { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}