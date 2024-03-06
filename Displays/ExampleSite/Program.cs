using ExampleSite.Components;
using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using MaterialDesign.Theming.Web.Setup;
using MaterialDesign.Web.Services;
using MudBlazor.Services;

HCTA defaultColor = new(213.22, 12, 22.396);

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDynamicMaterialIconsToWebApplication()
    .AddDynamicHeadStorage()
    .AddDynamicComponentStorage()
    .AddMaterialThemeService(new Theme(defaultColor))
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