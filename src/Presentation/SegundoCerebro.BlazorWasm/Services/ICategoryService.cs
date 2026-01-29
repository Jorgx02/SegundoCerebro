// filepath: src/Presentation/SegundoCerebro.BlazorWasm/Services/ICategoryService.cs
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;

namespace SegundoCerebro.BlazorWasm.Services;

public interface ICategoryService : IApiService<CategoryDto, CreateCategoryDto, UpdateCategoryDto>
{
    Task<IEnumerable<CategoryDto>> GetByTypeAsync(CategoryType type);
    Task<IEnumerable<CategoryDto>> GetActiveAsync();
}