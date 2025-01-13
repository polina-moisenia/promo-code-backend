using Microsoft.EntityFrameworkCore;
using PromoCodeService.Models;

namespace PromoCodeService.Data.Repositories;

public class PromoCodeRepository(PromoCodeDbContext context) : IPromoCodeRepository
{
    private readonly PromoCodeDbContext _context = context;

    public async Task AddPromoCodesAsync(IEnumerable<string> codes)
    {
        var promoCodes = codes.Select(code => new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
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
        promoCode.UpdatedAt = DateTime.UtcNow;
        _context.PromoCodes.Update(promoCode);
        await _context.SaveChangesAsync();
    }
}