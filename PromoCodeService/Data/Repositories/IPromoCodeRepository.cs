using Microsoft.EntityFrameworkCore.Storage;

namespace PromoCodeService.Data.Repositories;

public interface IPromoCodeRepository
{
     Task<IDbContextTransaction> BeginTransactionAsync();
     Task<bool> TryAddPromoCodeAsync(string code);
     Task<bool> UsePromoCodeAsync(string code);
}