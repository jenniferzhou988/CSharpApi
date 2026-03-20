namespace ShoppingCartAPI.Models;

public partial class UserRole : GDCTEntityBase<int>
{
    
    public string? RoleName { get; set; } 

    public virtual ICollection<User>? Users { get; set; } 

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? PublishTemplateToFlag { get; set; }

}
