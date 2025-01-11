using Microsoft.EntityFrameworkCore;
using PromoCodeService.Models;

namespace PromoCodeService.Data.Repositories;

public class PromoCodeRepository(PromoCodeDbContext context) : IPromoCodeRepository
{
    private readonly PromoCodeDbContext _context = context;

    public async Task<bool> IsCodeExistsAsync(string code)
    {
        return await _context.PromoCodes.AnyAsync(p => p.Code == code);
    }

    public async Task AddPromoCodesAsync(IEnumerable<string> codes, string requestId)
    {
        var promoCodes = codes.Select(code => new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            RequestId = requestId,
            IsActive = true,
            ExpiryDate = DateTime.UtcNow.AddMonths(1),
            DiscountAmount = 10
        });

        await _context.PromoCodes.AddRangeAsync(promoCodes);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<string>> GetCodesByRequestIdAsync(string requestId)
    {
        return await _context.PromoCodes
            .Where(p => p.RequestId == requestId)
            .Select(p => p.Code)
            .ToListAsync();
    }
}

