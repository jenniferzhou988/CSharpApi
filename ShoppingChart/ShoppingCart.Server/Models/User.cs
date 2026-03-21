namespace ShoppingCartAPI.Models;

public partial class User : GDCTEntityBase<int>
{
    public string? FirstName { get; set; }

    public string? LastName { get; set; } 

    public string? FullName { get; set; }
    public string? UserName { get; set; }

    public string? PasswordHash { get; set; }    
    public string? Email { get; set; } 

    public List<RefreshToken> RefreshTokens { get; set; } = new();

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    public UserCustomerLink? UserCustomerLink { get; set; }
}
