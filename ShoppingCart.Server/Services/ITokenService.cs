using ShoppingCartAPI.Contracts;
using ShoppingCartAPI.Models;

namespace ShoppingCartAPI.Services
{
    public interface ITokenService
    {
        (string token, DateTime expiresUtc) CreateAccessToken(User user);
        Task<(string accessToken, string refreshToken, DateTime expiry)> IssueTokenPairAsync(User user, CancellationToken ct = default);
        Task<AuthResponse> RotateRefreshTokenAsync(string presentedRefreshToken, CancellationToken ct = default);
        Task RevokeRefreshTokenAsync(string presentedRefreshToken, string reason = "logout", CancellationToken ct = default);
        Task RevokeAllForUserAsync(int userId, CancellationToken ct = default);

    }
}
