using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Data.Repositories;
using PromoCodeService.Services;

namespace PromoCodeService.Hubs;

public class PromoCodeGenerationHub(
    IPromoCodeRepository repository,
    IPromoCodeValidator validator,
    IPromoCodeGenerator generator) : Hub
{
    private readonly IPromoCodeRepository _repository = repository;
    private readonly IPromoCodeValidator _validator = validator;
    private readonly IPromoCodeGenerator _generator = generator;

    public async Task<bool> GeneratePromoCodes(ushort count, byte length)
    {
        try
        {
            if (!_validator.ValidateGenerationParameters(count, length))
            {
                return false;
            }

            var generatedCodes = GetPromoCodes(count, length).ToList();
            await _repository.AddPromoCodesAsync(generatedCodes);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GeneratePromoCodes: {ex.Message}");
            return false;
        }
    }

    public IEnumerable<string> GetPromoCodes(ushort count, byte length)
    {
        for (int i = 0; i < count; i++)
        {
            yield return _generator.GeneratePromoCode(length);
        }
    }
}
