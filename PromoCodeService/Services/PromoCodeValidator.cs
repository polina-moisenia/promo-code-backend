using PromoCodeService.Services;

public class PromoCodeValidator : IPromoCodeValidator
{
    public bool ValidateGenerationParameters(ushort count, byte length)
    {
        return count <= 2000 && (length == 7 || length == 8);
    }

    public bool ValidatePromoCode(string code)
    {
        return !string.IsNullOrWhiteSpace(code) && code.Length <= 8;
    }
}