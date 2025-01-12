using Microsoft.EntityFrameworkCore;
using PromoCodeService.Models;

namespace PromoCodeService.Data.Repositories;

public class PromoCodeRepository : IPromoCodeRepository
{
    private readonly PromoCodeDbContext _context;

    public PromoCodeRepository(PromoCodeDbContext context)
    {
        _context = context;
    }

    public async Task AddPromoCodesAsync(IEnumerable<string> codes, string requestId)
    {
        var promoCodes = codes.Select(code => new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            RequestId = requestId,
            IsActive = true
        });

        await _context.PromoCodes.AddRangeAsync(promoCodes);
        await _context.SaveChangesAsync();
    }

    public async Task<PromoCode?> GetPromoCodeByCodeAsync(string code)
    {
        return await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
    }

    public async Task UpdatePromoCodeAsync(PromoCode promoCode)
    {
        _context.PromoCodes.Update(promoCode);
        await _context.SaveChangesAsync();
    }
}