using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Data.Repositories;
using PromoCodeService.Services;

namespace PromoCodeService.Hubs;

public class PromoCodeGenerationHub : Hub
{
    private readonly IPromoCodeRepository _repository;
    private readonly IPromoCodeValidator _validator;
    private readonly IPromoCodeGenerator _generator;

    public PromoCodeGenerationHub(
        IPromoCodeRepository repository,
        IPromoCodeValidator validator,
        IPromoCodeGenerator generator)
    {
        _repository = repository;
        _validator = validator;
        _generator = generator;
    }

    public async Task<bool> GeneratePromoCodes(ushort count, byte length)
    {
        if (!_validator.ValidateGenerationParameters(count, length))
        {
            await Clients.Caller.SendAsync("GenerationResult", false);
            return false;
        }

        var requestId = Context.ConnectionId;
        var generatedCodes = new List<string>();

        for (int i = 0; i < count; i++)
        {
            var code = _generator.GeneratePromoCode(length);
            generatedCodes.Add(code);
        }

        await _repository.AddPromoCodesAsync(generatedCodes, requestId);
        await Clients.Caller.SendAsync("GenerationResult", true);

        foreach (var promoCode in generatedCodes)
        {
            await Clients.Caller.SendAsync("ReceivePromoCode", promoCode);
        }

        return true;
    }
}
