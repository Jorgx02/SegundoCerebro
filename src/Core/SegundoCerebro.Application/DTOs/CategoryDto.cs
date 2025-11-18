using SegundoCerebro.Domain.Enums;

namespace SegundoCerebro.Application.DTOs;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public string Icon { get; set; } = "fas fa-folder";
    public CategoryType Type { get; set; }
    public string TypeName => Type.ToString();
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
    
    public List<CategoryDto> SubCategories { get; set; } = new();
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public string Icon { get; set; } = "fas fa-folder";
    public CategoryType Type { get; set; }
    public Guid? ParentCategoryId { get; set; }
}

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public string Icon { get; set; } = "fas fa-folder";
    public CategoryType Type { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public bool IsActive { get; set; }
}