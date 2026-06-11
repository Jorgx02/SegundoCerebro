using SegundoCerebro.Domain.Common;
using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class Budget : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;

    // Relación con el usuario propietario
    public string UserId { get; set; } = string.Empty;

    // Relationships
    public Guid CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    public Guid? AccountId { get; set; }
    public Account? Account { get; set; }

    // Calculated property
    public decimal Spent { get; set; }
}