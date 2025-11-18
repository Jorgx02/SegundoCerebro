using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class Budget
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Spent { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign Keys
    public Guid CategoryId { get; set; }
    public Guid? AccountId { get; set; }
    
    // Navigation properties
    public Category Category { get; set; } = null!;
    public Account? Account { get; set; }
    
    // Calculated properties
    public decimal Remaining => Amount - Spent;
    public decimal PercentageUsed => Amount > 0 ? (Spent / Amount) * 100 : 0;
    public bool IsOverBudget => Spent > Amount;
}