using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MaterialDesign.Web.Services;
using MaterialDesign.Color.Colorspaces;
using ExampleSite.Components;
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
            .AddDynamicMaterialIconsToWebApplication()
            .AddTransient<SettingsStorageService>()
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