using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Icons;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MusicApp;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("body");
        await builder
            .AddDynamicMaterialIcons()
            .AddScoped(delegate { return SongInfo.Empty; })
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
            .AddMaterialThemeService((themeBuilder, services) =>
            {
                var info = services.GetRequiredService<SongInfo>();
                
                themeBuilder.UsingImage(imageBuilder => imageBuilder.WithStream(streamSource =>
                    streamSource.FromStreamMethod(() => new HttpClient().GetStreamAsync(info.AlbumCoverUrl))));
                return Task.FromResult(themeBuilder.Build());
            }, new Theme(new HCTA(177.1, 62.585, 89.691)));

        await builder.Build().RunAsync();
    }
}