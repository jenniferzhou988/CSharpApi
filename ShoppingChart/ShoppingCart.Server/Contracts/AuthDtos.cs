using ShoppingCartAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCartAPI.Contracts
{
    public record RegisterRequest(
        [Required, EmailAddress]
        string Email, 
         [Required, MinLength(8), MaxLength(128)]
        string Password, string? FullName, string FirstName, string LastName, int OrgId);
    public record LoginRequest(
        [Required, EmailAddress]
        string Email,
        [Required, MinLength(8), MaxLength(128)]
        string Password);
    //public record AuthResponse(string AccessToken, int ExpiresIn ,string TokenType, string? RefreshToken, DateTime Expires,User User);

    public record AuthResponse(
        int UserId,
        string Email,
        string AccessToken,
        string RefreshToken,
        DateTime Expires
    );

    public class AuthDtos
    {
    }
 

}
