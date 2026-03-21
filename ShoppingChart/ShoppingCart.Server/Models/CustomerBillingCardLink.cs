using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("CustomerBillingCardLink")]
    public class CustomerBillingCardLink : GDCTEntityBase<int>
    {
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public int BankCardInfoId { get; set; }

        [ForeignKey(nameof(BankCardInfoId))]
        public BankCardInfo BankCardInfo { get; set; } = null!;
    }
}