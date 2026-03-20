using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AngularApplication.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ProductName { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [MaxLength(1000)]
        public string? Description { get; set; }

        public DateTime Created { get; set; }

        [MaxLength(256)]
        public string? CreatedBy { get; set; }

        public DateTime? Modified { get; set; }

        [MaxLength(256)]
        public string? ModifiedBy { get; set; }
    }
}