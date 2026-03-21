using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("CustomerAddressLink")]
    public class CustomerAddressLink : GDCTEntityBase<int>
    {
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public int AddressId { get; set; }

        [ForeignKey(nameof(AddressId))]
        public Address Address { get; set; } = null!;

        public int AddressTypeId { get; set; }

        [ForeignKey(nameof(AddressTypeId))]
        public AddressType AddressType { get; set; } = null!;
    }
}