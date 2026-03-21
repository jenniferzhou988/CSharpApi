using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShoppingCartAPI.Services;

namespace ShoppingCartAPI.Data
{
    public class EncryptedConverter : ValueConverter<string, string>
    {
        public EncryptedConverter(IEncryptionService encryptionService)
            : base(
                v => encryptionService.Encrypt(v),
                v => encryptionService.Decrypt(v))
        {
        }
    }
}