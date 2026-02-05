// filepath: src/Presentation/SegundoCerebro.BlazorWasm/Services/CategoryService.cs
using SegundoCerebro.BlazorWasm.Models;
using SegundoCerebro.BlazorWasm.Models.Enums;
using System.Net.Http.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public class CategoryService : ApiService<CategoryDto, CreateCategoryDto, UpdateCategoryDto>, ICategoryService
{
    public CategoryService(HttpClient httpClient) : base(httpClient, "categories")
    {
    }

    public async Task<IEnumerable<CategoryDto>> GetByTypeAsync(CategoryType type)
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<CategoryDto>>($"api/categories/type/{(int)type}");
        return result ?? Enumerable.Empty<CategoryDto>();
    }

    public async Task<IEnumerable<CategoryDto>> GetActiveAsync()
    {
        var allCategories = await GetAllAsync();
        return allCategories.Where(c => c.IsActive);
    }
}