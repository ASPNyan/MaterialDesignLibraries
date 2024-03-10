using ExampleSite.Components;
using ExampleSite.Data;
using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MaterialDesign.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

namespace ExampleSite;

internal class Program
{
    internal static readonly HCTA DefaultColor = new(213.22, 12, 22.396);

    public static async Task Main(string[] args)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
        builder.Services
            .AddDynamicMaterialIconsToWebApplication()
            .AddDynamicHeadStorage()
            .AddDynamicComponentStorage()
            .AddMaterialThemeService(new Theme(DefaultColor))
            .AddMudServices()
            .AddRazorComponents()
            .AddInteractiveServerComponents();

        SqliteConnection dbConnection = new SqliteConnection(@"DataSource=Data\app.db;Cache=shared");

        builder.Services.AddDbContext<IdentityDb>(opt => opt.UseSqlite(dbConnection));

        builder.Services
            .AddCascadingAuthenticationState();

        builder.Services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<IdentityDb>()
            .AddSignInManager();
    
        builder.Services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();


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