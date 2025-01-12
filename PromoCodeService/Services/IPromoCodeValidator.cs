namespace PromoCodeService.Services;

public interface IPromoCodeValidator
{
    bool ValidateGenerationParameters(ushort count, byte length);
    bool ValidatePromoCode(string code);
}