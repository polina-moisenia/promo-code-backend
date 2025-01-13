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

            await NotifyClientWithGeneratedCodes(generatedCodes);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GeneratePromoCodes: {ex.Message}");
            return false;
        }
    }

    private IEnumerable<string> GetPromoCodes(ushort count, byte length)
    {
        for (int i = 0; i < count; i++)
        {
            yield return _generator.GeneratePromoCode(length);
        }
    }

    private async Task NotifyClientWithGeneratedCodes(IEnumerable<string> codes)
    {
        // Send generated codes to the client
        // (not part of the task requirements, added for testing and frontend display)
        await Clients.Caller.SendAsync("CodesGenerated", codes);
    }
}
