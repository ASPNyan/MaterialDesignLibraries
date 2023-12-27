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
        builder
            .AddDynamicMaterialIcons()
            .AddScoped(delegate { return new SongInfo
            {
                Name = "a",
                Album = "Album",
                Author = "a",
                AlbumCoverUrl = "https://raw.githubusercontent.com/julien-gargot/images-placeholder/master/placeholder-square.png"
            }; })
            .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
            .AddMaterialThemeService();
        builder.RootComponents.Add<App>("body");

        var host = builder.Build();
        await host.SetMaterialThemeService((themeBuilder, services) =>
        {
            var info = services.GetRequiredService<SongInfo>();

            themeBuilder.UsingImage(imageBuilder => imageBuilder.WithStream(streamSource =>
                streamSource.FromStreamMethod(() => new HttpClient().GetStreamAsync(info.AlbumCoverUrl))));
            return Task.FromResult(themeBuilder.Build());
        }, new Theme(new HCTA(177.1, 62.585, 89.691)));
        await host.RunAsync();
    }
}