using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Models;

public class CategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CategoryType Type { get; set;}
    public string TypeName => Type.ToString();
    public string Color { get; set; } = "#6366F1";
    public string Icon { get; set; } = "fas fa-folder";
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? ParentCategoryId { get; set; }
    public string? ParentCategoryName { get; set; }
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CategoryType Type { get; set;}
    public string Color { get; set; } = "#6366F1";
    public string Icon { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
}

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CategoryType Type { get; set; }
    public string Color { get; set; } = "#6366F1";
    public string Icon { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public Guid? ParentCategoryId { get; set; }
}