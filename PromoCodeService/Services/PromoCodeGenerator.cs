using System.Security.Cryptography;
using System.Text;

namespace PromoCodeService.Services;

public class PromoCodeGenerator : IPromoCodeGenerator
{
    public string GeneratePromoCode(byte length)
    {
        using var sha256 = SHA256.Create();
        var guid = Guid.NewGuid().ToString();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(guid));
        var base64Hash = Convert.ToBase64String(hash)
            .Replace("+", string.Empty)
            .Replace("/", string.Empty)
            .Replace("=", string.Empty);

        return base64Hash.Substring(0, length);
    }
}
