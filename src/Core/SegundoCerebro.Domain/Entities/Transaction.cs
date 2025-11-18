using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class Transaction
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign Keys
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
    
    // Navigation properties
    public Account Account { get; set; } = null!;
    public Category Category { get; set; } = null!;
}