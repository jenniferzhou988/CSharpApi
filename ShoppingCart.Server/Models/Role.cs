namespace ShoppingCartAPI.Models;

public partial class Role : GDCTEntityBase<int>
{
    public string? RoleName { get; set; }

    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}