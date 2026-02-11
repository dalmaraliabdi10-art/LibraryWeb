using LibraryWeb.Components;
using Microsoft.EntityFrameworkCore;
using LibraryWeb.Data;
using LibraryWeb.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));
    
// Koppla Databas
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// Koppla Services
builder.Services.AddScoped<LibraryService>();
builder.Services.AddScoped<UserSession>(); // Lägger till UserSession som en scoped service, så att varje användare får sin egen session

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();