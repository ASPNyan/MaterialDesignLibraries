using MaterialDesign.Icons;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MaterialDesign.Web.Services;
using ExampleSite.Components;
using ExampleSite.Components.Settings;
using ExampleSite.DefaultThemes;
using ExampleSite.Services;
using MudBlazor.Services;

namespace ExampleSite;

internal class Program
{
    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

        builder.Services.AddServerSideBlazor();
        
        // Add services to the container.
        builder.Services
            .AddDynamicMaterialIconsToWebApplication(style => $"fonts/MaterialSymbols{style}.woff2")
            .AddTransient<SettingsStorageService>()
            .AddScoped<LayoutSettings>()
            .AddMaterialThemeService(new Oceanic())
            .AddDynamicHeadStorage()
            .AddDynamicComponentStorage()
            .AddMudServices()
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        ThemeSetup.Setup();

        WebApplication app = builder.Build();

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

        await app.Services.SetDefaultThemeMode(defaultIsDark: true);
        
        await app.RunAsync();
    }
}