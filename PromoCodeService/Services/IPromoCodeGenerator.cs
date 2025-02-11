namespace PromoCodeService.Services;

public interface IPromoCodeGenerator
{
    string GeneratePromoCode(byte length);
}