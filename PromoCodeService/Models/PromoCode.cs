using System.ComponentModel.DataAnnotations;

namespace PromoCodeService.Models;

public class PromoCode
{
    [Key]
    public Guid Id { get; init; }

    [MaxLength(50)]
    public required string Code { get; init; }

    public DateTime ExpiryDate { get; init; }

    public bool IsActive { get; init; }

    public decimal DiscountAmount { get; init; }

    public required string RequestId { get; init; }
}
