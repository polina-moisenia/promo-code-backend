using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Data.Repositories;
using PromoCodeService.Services;

namespace PromoCodeService.Hubs;

public class PromoCodeUsageHub(IPromoCodeRepository repository, IPromoCodeValidator validator) : Hub
{
    private readonly IPromoCodeRepository _repository = repository;
    private readonly IPromoCodeValidator _validator = validator;

    public async Task<bool> UsePromoCode(string code)
    {
        try
        {
            if (!_validator.ValidatePromoCode(code))
            {
                return false;
            }

            return await _repository.UsePromoCodeAsync(code);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in UseCode: {ex.Message}");
            return false;
        }
    }
}
