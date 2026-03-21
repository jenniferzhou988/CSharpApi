using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("BankCardInfo")]
    public class BankCardInfo : GDCTEntityBase<int>
    {
        /// <summary>
        /// Full card number — stored encrypted (two-way AES) in the database.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string CardNo { get; set; } = null!;

        /// <summary>
        /// Last 4 digits of the card number — stored in plain text for display purposes.
        /// </summary>
        [Required]
        [MaxLength(4)]
        public string Last4Digit { get; set; } = null!;

        /// <summary>
        /// Card verification code — stored encrypted (two-way AES) in the database.
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string CVC { get; set; } = null!;

        [Required]
        public int ExpiryMonth { get; set; }

        [Required]
        public int ExpiryYear { get; set; }

        // Foreign key to BillingMethod
        public int BillingMethodId { get; set; }

        [ForeignKey(nameof(BillingMethodId))]
        public BillingMethod BillingMethod { get; set; } = null!;

        public ICollection<CustomerBillingCardLink> CustomerBillingCardLinks { get; set; } = new List<CustomerBillingCardLink>();
    }
}