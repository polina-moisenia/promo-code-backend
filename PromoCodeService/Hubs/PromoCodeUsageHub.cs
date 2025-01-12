using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Data.Repositories;
using PromoCodeService.Services;

namespace PromoCodeService.Hubs;

public class PromoCodeUsageHub : Hub
{
    private readonly IPromoCodeRepository _repository;
    private readonly IPromoCodeValidator _validator;

    public PromoCodeUsageHub(IPromoCodeRepository repository, IPromoCodeValidator validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<bool> UseCode(string code)
    {
        if (!_validator.ValidatePromoCode(code))
        {
            return false;
        }

        var promoCode = await _repository.GetPromoCodeByCodeAsync(code);

        if (promoCode == null || !promoCode.IsActive)
        {
            return false;
        }

        promoCode.IsActive = false;
        await _repository.UpdatePromoCodeAsync(promoCode);

        return true;
    }
}
