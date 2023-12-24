using MaterialDesign.Icons;
using MaterialDesign.Theming.Injection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MusicApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("body");
        (await builder.AddDynamicMaterialIcons()
            .AddMaterialThemeService((themeBuilder, services) =>
            {
                
            }))
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

        await builder.Build().RunAsync();
    }
}