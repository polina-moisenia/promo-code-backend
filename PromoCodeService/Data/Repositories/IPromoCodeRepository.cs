using PromoCodeService.Models;

namespace PromoCodeService.Data.Repositories;

public interface IPromoCodeRepository
{
     Task AddPromoCodesAsync(IEnumerable<string> codes, string requestId);
     Task<PromoCode?> GetPromoCodeByCodeAsync(string code);
     Task UpdatePromoCodeAsync(PromoCode promoCode);
}