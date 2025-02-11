namespace PromoCodeService.Services;

public class PromoCodeGenerator : IPromoCodeGenerator
{
    private readonly Random _random = new();
    private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public string GeneratePromoCode(byte length)
    {
        char[] buffer = new char[length];
        for (int i = 0; i < length; i++)
        {
            buffer[i] = _chars[_random.Next(_chars.Length)];
        }

        return new string(buffer);
    }
}
