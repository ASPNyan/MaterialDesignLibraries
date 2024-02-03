using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MaterialDesign.Web.Services;
using MaterialDesignCustomSchemes;
using MudBlazor.Services;
using MaterialDesignCustomSchemes.Components;

HCTA defaultThemeSource = new(308.35, 53.78, 28.95);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddScoped<CustomScheme>(_ => new CustomScheme(defaultThemeSource))
    .AddDynamicMaterialIconsToWebApplication()
    .AddMaterialThemeService(new Theme(defaultThemeSource))
    .AddDynamicHeadStorage()
    .AddDynamicComponentStorage()
    .AddMudServices()
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

await app.Services.SetDefaultThemeMode(defaultIsDark: true);

app.Run();
