using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("Customer")]
    public class Customer : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; } = null!;

        [MaxLength(100)]
        public string? MiddleName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; } = null!;

        [MaxLength(256)]
        public string? Email { get; set; }

        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public ICollection<CustomerAddressLink> CustomerAddressLinks { get; set; } = new List<CustomerAddressLink>();

        public ICollection<CustomerBillingCardLink> CustomerBillingCardLinks { get; set; } = new List<CustomerBillingCardLink>();

        public ICollection<ShoppingCart> ShoppingCarts { get; set; } = new List<ShoppingCart>();

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public UserCustomerLink? UserCustomerLink { get; set; }
    }
}