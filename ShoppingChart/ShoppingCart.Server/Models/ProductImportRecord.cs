using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ProductImportRecord")]
    public class ProductImportRecord : GDCTEntityBase<int>
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ImportPrice { get; set; }

        [Required]
        public int Quantity { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}