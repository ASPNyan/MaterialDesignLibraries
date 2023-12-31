using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MusicApp.Components;

namespace MusicApp;

internal static class Program
{
    internal static readonly HCTA DefaultThemeSource = new(308.35, 53.78, 28.95);
    
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        builder.Services
            .AddDynamicMaterialIconsToWebApplication()
            .AddMaterialThemeService(new Theme(DefaultThemeSource))
            .AddScoped<SongInfoContainer>()
            .AddScoped<HttpClient>()
            .AddRazorComponents()
            .AddInteractiveServerComponents();
        
        ThemeSetup.Setup();

        var app = builder.Build();

// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error", createScopeForErrors: true);
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseStaticFiles();
        app.UseAntiforgery();

        app.MapRazorComponents<App>()
            .AddInteractiveServerRenderMode();
        
        await app.Services.SetDefaultThemeMode(defaultIsDark: true); // my eyes

        await app.RunAsync();
    }
}