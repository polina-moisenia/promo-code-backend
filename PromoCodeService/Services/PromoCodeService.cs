using Microsoft.EntityFrameworkCore.Storage;
using PromoCodeService.Data.Repositories;

namespace PromoCodeService.Services;

public class PromoCodeCreateService : IPromoCodeService
{
    private readonly IPromoCodeRepository _repository;
    private readonly IPromoCodeGenerator _generator;

    public PromoCodeCreateService(IPromoCodeRepository repository, IPromoCodeGenerator generator)
    {
        _repository = repository;
        _generator = generator;
    }

    public async Task<List<string>> GeneratePromoCodesAsync(ushort count, byte length)
    {
        var generatedCodes = new List<string>(count);
        int insertedCount = 0;

        await using (IDbContextTransaction transaction = await _repository.BeginTransactionAsync())
        {
            try
            {
                while (insertedCount < count)
                {
                    var nextCode = _generator.GeneratePromoCode(length);
                    var added = await _repository.TryAddPromoCodeAsync(nextCode);

                    if (added)
                    {
                        generatedCodes.Add(nextCode);
                        insertedCount++;
                    }
                    
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        return generatedCodes;
    }
}
