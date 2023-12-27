using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Theming;
using MaterialDesignShowcase.Components;
using MaterialDesign.Theming.Injection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMaterialThemeService(new Theme(new HCTA(293.42, 74.248, 34.458))) // blazor blue (#512bd4)
    .AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

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
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(MaterialDesignShowcase.Client.Pages.Palettes).Assembly);

app.Run();