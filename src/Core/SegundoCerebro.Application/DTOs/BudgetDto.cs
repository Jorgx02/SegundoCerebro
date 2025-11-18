using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

public class BudgetDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public decimal Spent { get; set; }
    public BudgetPeriod Period { get; set; }
    public string PeriodName => Period.ToString();
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    
    public Guid? AccountId { get; set; }
    public string? AccountName { get; set; }
    
    // Calculated properties
    public decimal Remaining => Amount - Spent;
    public decimal PercentageUsed => Amount > 0 ? (Spent / Amount) * 100 : 0;
    public bool IsOverBudget => Spent > Amount;
}

public class CreateBudgetDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? AccountId { get; set; }
}

public class UpdateBudgetDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public BudgetPeriod Period { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Guid CategoryId { get; set; }
    public Guid? AccountId { get; set; }
    public bool IsActive { get; set; }
}