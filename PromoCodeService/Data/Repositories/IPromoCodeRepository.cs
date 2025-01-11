namespace PromoCodeService.Data.Repositories;

public interface IPromoCodeRepository
{
     Task<bool> IsCodeExistsAsync(string code);
     Task AddPromoCodesAsync(IEnumerable<string> codes, string requestId);
     Task<IEnumerable<string>> GetCodesByRequestIdAsync(string requestId);
}