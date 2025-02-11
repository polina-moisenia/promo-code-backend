namespace PromoCodeService.Services;

public interface IPromoCodeService
{
    Task<List<string>> GeneratePromoCodesAsync(ushort count, byte length);
}