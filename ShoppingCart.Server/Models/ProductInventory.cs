using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ProductInventory")]
    public class ProductInventory : GDCTEntityBase<int>
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required]
        public int Quantity { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}