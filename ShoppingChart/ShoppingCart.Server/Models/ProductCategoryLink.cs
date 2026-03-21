using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ProductCategoryLink")]
    public class ProductCategoryLink : GDCTEntityBase<int>
    {
        public int ProductId { get; set; }

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;

        public int ProductCategoryId { get; set; }

        [ForeignKey(nameof(ProductCategoryId))]
        public ProductCategory ProductCategory { get; set; } = null!;
    }
}