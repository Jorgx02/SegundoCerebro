using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

public class TransactionDto
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public string TypeName => Type.ToString();
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Guid AccountId { get; set; }
    public string AccountName { get; set; } = string.Empty;
    
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
}

public class CreateTransactionDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
}

public class UpdateTransactionDto
{
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public TransactionType Type { get; set; }
    public DateTime Date { get; set; }
    public string? Reference { get; set; }
    public string? Notes { get; set; }
    public Guid AccountId { get; set; }
    public Guid CategoryId { get; set; }
}