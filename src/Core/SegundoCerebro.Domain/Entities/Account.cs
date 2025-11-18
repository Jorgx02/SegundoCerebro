using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class Account
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; } = "EUR";
    public string? BankName { get; set; }
    public string? AccountNumber { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Navigation properties
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}