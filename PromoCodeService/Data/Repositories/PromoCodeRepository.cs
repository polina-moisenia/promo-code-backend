using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using PromoCodeService.Models;

namespace PromoCodeService.Data.Repositories;

public class PromoCodeRepository(PromoCodeDbContext context) : IPromoCodeRepository
{
    private readonly PromoCodeDbContext _context = context;

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }
    
    public async Task<bool> TryAddPromoCodeAsync(string code)
    {
        var promoCode = new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = code,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

         try
        {
            await _context.PromoCodes.AddAsync(promoCode);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgException && pgException.SqlState == "23505")
            {
                _context.Entry(promoCode).State = EntityState.Detached;
                return false;
            }
            throw;
        }
    }

    public async Task<bool> UsePromoCodeAsync(string code)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            var promoCode = await _context.PromoCodes.FirstOrDefaultAsync(p => p.Code == code);
            if (promoCode == null || !promoCode.IsActive)
            {
                return false;
            }

            promoCode.IsActive = false;
            promoCode.UpdatedAt = DateTime.UtcNow;

            _context.PromoCodes.Update(promoCode);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return true;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}