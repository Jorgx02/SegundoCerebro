using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SegundoCerebro.BlazorWasm;
using SegundoCerebro.BlazorWasm.Services;
using SegundoCerebro.BlazorWasm.Providers;
using MudBlazor.Services;
using Blazored.LocalStorage;
using FluentValidation;
using Microsoft.AspNetCore.Components.Authorization;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar el interceptor de tokens JWT
builder.Services.AddTransient<JwtAuthorizationMessageHandler>();

// Configurar el HttpClient para que use el interceptor
builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<JwtAuthorizationMessageHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler) { BaseAddress = new Uri("http://localhost:7099/") };
});

// MudBlazor
builder.Services.AddMudServices();

// Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Services
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IBudgetService, BudgetService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITodoItemService, TodoItemService>();
builder.Services.AddScoped<ICardService, CardService>();
builder.Services.AddScoped<IHabitService, HabitService>();
builder.Services.AddScoped<ITimeLogService, TimeLogService>();

await builder.Build().RunAsync();
