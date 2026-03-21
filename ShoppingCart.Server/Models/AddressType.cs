using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("AddressType")]
    public class AddressType : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string TypeName { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<CustomerAddressLink> CustomerAddressLinks { get; set; } = new List<CustomerAddressLink>();
    }
}