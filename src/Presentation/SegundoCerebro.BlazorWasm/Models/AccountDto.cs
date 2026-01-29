using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Models;

public class AccountDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
    public string TypeName => Type.ToString();
    public string Currency { get; set; } = "EUR";
    public decimal Balance { get; set; }
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = "EUR";
    public decimal InitialBalance { get; set; }
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
}

public class UpdateAccountDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public AccountType Type { get; set; }
    public string Currency { get; set; } = "EUR";
    public string? AccountNumber { get; set; }
    public string? BankName { get; set; }
    public bool IsActive { get; set; }
}