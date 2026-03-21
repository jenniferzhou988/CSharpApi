using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    public class Product : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = null!;

        [MaxLength(50)]
        public string? SKU { get; set; }

        [MaxLength(12)]
        public string? UPC { get; set; }

        [MaxLength(200)]
        public string? BrandName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();

        public ICollection<ProductCategoryLink> ProductCategoryLinks { get; set; } = new List<ProductCategoryLink>();

        public ICollection<ShoppingCartDetail> ShoppingCartDetails { get; set; } = new List<ShoppingCartDetail>();

        public ICollection<ProductImportRecord> ProductImportRecords { get; set; } = new List<ProductImportRecord>();

        public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}