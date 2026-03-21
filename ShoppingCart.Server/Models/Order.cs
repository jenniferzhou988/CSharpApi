using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoppingCartAPI.Models
{
    [Table("Order")]
    public class Order : GDCTEntityBase<int>
    {
        public int CustomerId { get; set; }

        [ForeignKey(nameof(CustomerId))]
        public Customer Customer { get; set; } = null!;

        public int BankCardId { get; set; }

        [ForeignKey(nameof(BankCardId))]
        public BankCardInfo BankCardInfo { get; set; } = null!;

        public int ShippingAddressId { get; set; }

        [ForeignKey(nameof(ShippingAddressId))]
        public Address ShippingAddress { get; set; } = null!;

        public int BillingAddressId { get; set; }

        [ForeignKey(nameof(BillingAddressId))]
        public Address BillingAddress { get; set; } = null!;

        public int OrderStatusId { get; set; }

        [ForeignKey(nameof(OrderStatusId))]
        public OrderStatus OrderStatus { get; set; } = null!;

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}