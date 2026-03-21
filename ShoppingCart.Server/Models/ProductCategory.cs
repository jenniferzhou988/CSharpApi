using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("ProductCategory")]
    public class ProductCategory : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string CategoryName { get; set; } = null!;

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ICollection<ProductCategoryLink> ProductCategoryLinks { get; set; } = new List<ProductCategoryLink>();
    }
}