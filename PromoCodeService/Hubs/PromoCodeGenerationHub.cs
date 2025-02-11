using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Services;

namespace PromoCodeService.Hubs;

public class PromoCodeGenerationHub(IPromoCodeService promoCodeService, IPromoCodeValidator validator) : Hub
{
    private readonly IPromoCodeService _promoCodeService = promoCodeService;
    private readonly IPromoCodeValidator _validator = validator;

    public async Task<bool> GeneratePromoCodes(ushort count, byte length)
    {
        try
        {
            if (!_validator.ValidateGenerationParameters(count, length))
            {
                return false;
            }

            var generatedCodes = await _promoCodeService.GeneratePromoCodesAsync(count, length);
            await NotifyClientWithGeneratedCodes(generatedCodes);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in GeneratePromoCodes: {ex.Message}");
            return false;
        }
    }

    private async Task NotifyClientWithGeneratedCodes(IEnumerable<string> codes)
    {
        // Send generated codes to the client
        // (not part of the task requirements, added for testing and frontend display)
        await Clients.Caller.SendAsync("CodesGenerated", codes);
    }
}
