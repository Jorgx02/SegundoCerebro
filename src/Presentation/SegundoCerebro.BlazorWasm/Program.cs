using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SegundoCerebro.BlazorWasm;
using SegundoCerebro.BlazorWasm.Services;
using MudBlazor.Services;
using Blazored.LocalStorage;
using FluentValidation;
using System.Reflection;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HTTP Client con la URL correcta del API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:7099/") });

// MudBlazor
builder.Services.AddMudServices();

// Blazored LocalStorage
builder.Services.AddBlazoredLocalStorage();

// FluentValidation
builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

// Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ITransactionService, TransactionService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

await builder.Build().RunAsync();