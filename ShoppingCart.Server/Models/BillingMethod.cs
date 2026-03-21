using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("BillingMethod")]
    public class BillingMethod : GDCTEntityBase<int>
    {
        [Required]
        [MaxLength(100)]
        public string BillingMethodName { get; set; } = null!;

        [MaxLength(500)]
        public string? Description { get; set; }

        public ICollection<BankCardInfo> BankCardInfos { get; set; } = new List<BankCardInfo>();
    }
}