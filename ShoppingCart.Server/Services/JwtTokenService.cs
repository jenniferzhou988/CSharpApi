
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoppingCartAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingCartAPI.Services
{
    // public class JwtTokenService(IOptions<JwtOptions> options)
    public class JwtTokenService
    {
        //private readonly JwtOptions _opt;
        private readonly byte[] _key;

        private readonly JwtOptions _opt;// = options.Value;

        public JwtTokenService(JwtOptions opt)
        {
            _opt = opt;
            _key = Encoding.UTF8.GetBytes(_opt.SigningKey);
        }

        public (string token, DateTime expiresUtc) CreateAccessToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_opt.SigningKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var now = DateTime.UtcNow;
            var expires = now.AddMinutes(_opt.AccessTokenMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email??string.Empty),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email??string.Empty),
            new("FullName",user.FullName??string.Empty),
        };
            
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,role.RoleName??""));
            }

            var jwt = new JwtSecurityToken(
                issuer: _opt.Issuer,
                audience: _opt.Audience,
                claims: claims,
                notBefore: now,
                expires: expires,
                signingCredentials: creds);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return (token, expires);
        }

    }

    public class JwtOptions
    {
        public string Issuer { get; set; } = default!;
        public string Audience { get; set; } = default!;
        public string SigningKey { get; set; } = default!;
        public int AccessTokenMinutes { get; set; } = 60; // default 1h
//        public int AccessTokenMinutes { get; set; } = 15;
        public int RefreshTokenDays { get; set; } = 7;

    }

}
