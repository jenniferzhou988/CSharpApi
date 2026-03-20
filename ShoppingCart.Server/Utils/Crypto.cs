using System.Security.Cryptography;
using System.Text;

namespace ShoppingCartAPI.Utils
{
    public static class Crypto
    {
        public static string Sha256(string input)
        {
            using var sha = SHA256.Create();
            var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            return Convert.ToHexString(bytes); // uppercase hex
        }

        public static string GenerateSecureRandomToken(int bytes = 64)
        {
            var data = RandomNumberGenerator.GetBytes(bytes);
            // Base64Url (no +, /, =)
            return Convert.ToBase64String(data)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }


    }
}
