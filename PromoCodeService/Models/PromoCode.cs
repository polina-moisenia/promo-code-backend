using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace PromoCodeService.Models;

[Index(nameof(Code), IsUnique = true)]
public class PromoCode
{
    [Key]
    public Guid Id { get; set; }

    public required string Code { get; set; }

    public bool IsActive { get; set; }

    public required string RequestId { get; set; }
}
