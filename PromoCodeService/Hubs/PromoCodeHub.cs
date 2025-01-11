using Microsoft.AspNetCore.SignalR;
using PromoCodeService.Data.Repositories;

namespace PromoCodeService.Hubs;

public class PromoCodeHub : Hub
{
    private readonly IPromoCodeRepository _repository;

    public PromoCodeHub(IPromoCodeRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> GeneratePromoCodes(ushort count, byte length)
    {
        var requestId = Context.ConnectionId;
        var generatedCodes = new List<string>();

        try
        {
            for (int i = 0; i < count; i++)
            {
                string code;
                do
                {
                    code = GenerateCode(length);
                } while (await _repository.IsCodeExistsAsync(code));

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
        catch (Exception)
        {
            await Clients.Caller.SendAsync("GenerationResult", false);
            return false;
        }
    }

    private string GenerateCode(byte length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}
