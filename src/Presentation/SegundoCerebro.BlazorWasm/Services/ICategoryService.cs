using SegundoCerebro.BlazorWasm.Models;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetAllAsync();
    Task<CategoryDto?> GetByIdAsync(Guid id);
    Task<CategoryDto> CreateAsync(CreateCategoryDto createDto);
    Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto updateDto);
    Task<bool> DeleteAsync(Guid id);
}

// Implementación temporal
public class CategoryService : ICategoryService
{
    public Task<IEnumerable<CategoryDto>> GetAllAsync()
    {
        // Mock categories por ahora
        var categories = new List<CategoryDto>
        {
            new() { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Food & Dining", Type = CategoryType.Expense, Icon = "restaurant", Color = "#FF5722" },
            new() { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Transportation", Type = CategoryType.Expense, Icon = "directions_car", Color = "#2196F3" },
            new() { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Shopping", Type = CategoryType.Expense, Icon = "shopping_cart", Color = "#9C27B0" },
            new() { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "Entertainment", Type = CategoryType.Expense, Icon = "movie", Color = "#E91E63" },
            new() { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Salary", Type = CategoryType.Income, Icon = "work", Color = "#4CAF50" },
            new() { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Freelance", Type = CategoryType.Income, Icon = "computer", Color = "#00BCD4" },
            new() { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Investment", Type = CategoryType.Income, Icon = "trending_up", Color = "#FF9800" }
        };

        return Task.FromResult(categories.AsEnumerable());
    }

    public Task<CategoryDto?> GetByIdAsync(Guid id) => throw new NotImplementedException();
    public Task<CategoryDto> CreateAsync(CreateCategoryDto createDto) => throw new NotImplementedException();
    public Task<CategoryDto> UpdateAsync(Guid id, UpdateCategoryDto updateDto) => throw new NotImplementedException();
    public Task<bool> DeleteAsync(Guid id) => throw new NotImplementedException();
}

public class CreateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CategoryType Type { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public string Icon { get; set; } = "folder";
}

public class UpdateCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public CategoryType Type { get; set; }
    public string Color { get; set; } = "#3B82F6";
    public string Icon { get; set; } = "folder";
    public bool IsActive { get; set; }
}