using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Domain.Entities;

public class Category
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6"; // Default blue color
    public string Icon { get; set; } = "fas fa-folder"; // Font Awesome icon
    public CategoryType Type { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    // Hierarchy support
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    
    // Navigation properties
    public ICollection<Category> SubCategories { get; set; } = new List<Category>();
    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    public ICollection<Budget> Budgets { get; set; } = new List<Budget>();
}