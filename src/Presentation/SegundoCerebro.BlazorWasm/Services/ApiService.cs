using System.Net.Http.Json;
using System.Text.Json;

namespace SegundoCerebro.BlazorWasm.Services;

public interface IApiService<TDto, TCreateDto, TUpdateDto>
{
    Task<IEnumerable<TDto>> GetAllAsync();
    Task<TDto?> GetByIdAsync(Guid id);
    Task<TDto> CreateAsync(TCreateDto createDto);
    Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto);
    Task<bool> DeleteAsync(Guid id);
}

public class ApiService<TDto, TCreateDto, TUpdateDto> : IApiService<TDto, TCreateDto, TUpdateDto>
{
    protected readonly HttpClient _httpClient;
    protected readonly string _endpoint;
    protected readonly JsonSerializerOptions _jsonOptions;

    public ApiService(HttpClient httpClient, string endpoint)
    {
        _httpClient = httpClient;
        _endpoint = endpoint;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    public async Task<IEnumerable<TDto>> GetAllAsync()
    {
        var response = await _httpClient.GetFromJsonAsync<IEnumerable<TDto>>($"api/{_endpoint}", _jsonOptions);
        return response ?? new List<TDto>();
    }

    public async Task<TDto?> GetByIdAsync(Guid id)
    {
        return await _httpClient.GetFromJsonAsync<TDto>($"api/{_endpoint}/{id}", _jsonOptions);
    }

    public async Task<TDto> CreateAsync(TCreateDto createDto)
    {
        var response = await _httpClient.PostAsJsonAsync($"api/{_endpoint}", createDto, _jsonOptions);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TDto>(_jsonOptions);
        return result!;
    }

    public async Task<TDto> UpdateAsync(Guid id, TUpdateDto updateDto)
    {
        var response = await _httpClient.PutAsJsonAsync($"api/{_endpoint}/{id}", updateDto, _jsonOptions);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TDto>(_jsonOptions);
        return result!;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync($"api/{_endpoint}/{id}");
        return response.IsSuccessStatusCode;
    }
}