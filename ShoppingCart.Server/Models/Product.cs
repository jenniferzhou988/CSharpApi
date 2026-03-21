using ShoppingCartAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularApplication.Models
{
    public class Product : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    }
}