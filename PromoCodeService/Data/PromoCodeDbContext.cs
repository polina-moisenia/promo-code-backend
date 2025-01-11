using Microsoft.EntityFrameworkCore;
using PromoCodeService.Models;

namespace PromoCodeService.Data;

public class PromoCodeDbContext(DbContextOptions<PromoCodeDbContext> options) : DbContext(options)
{
    public required DbSet<PromoCode> PromoCodes { get; set; }
}