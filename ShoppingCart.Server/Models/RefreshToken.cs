namespace ShoppingCartAPI.Models
{
    public class RefreshToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int UserId { get; set; }
        public User User { get; set; } = default!;
        // Store **hash** and **salt** of the refresh token (never store raw token)
        public string TokenHash { get; set; } = default!;
        public DateTimeOffset ExpiresAt { get; set; }
        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
        public bool Revoked { get; set; }
        public DateTimeOffset? RevokedAt { get; set; }
        public string? RevokedReason { get; set; }
        // For rotation linking
        public Guid? ReplacedByTokenId { get; set; }
    }
}
