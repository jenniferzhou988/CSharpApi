using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("Address")]
    public class Address : GDCTEntityBase<int>
    {
        [MaxLength(20)]
        public string? StreetNo { get; set; }

        [Required]
        [MaxLength(200)]
        public string Street { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string City { get; set; } = null!;

        [Required]
        [MaxLength(20)]
        public string PostalCode { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Province { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Country { get; set; } = null!;

        public ICollection<CustomerAddressLink> CustomerAddressLinks { get; set; } = new List<CustomerAddressLink>();
    }
}