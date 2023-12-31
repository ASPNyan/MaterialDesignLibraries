using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

public static class ServiceProviderExtensions
{
    public static async Task SetDefaultThemeMode(this IServiceProvider serviceProvider, bool defaultIsDark)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var container = scope.ServiceProvider.GetRequiredService<ThemeContainer>();
        
        if (defaultIsDark) container.Theme.SetDark();
        else container.Theme.SetLight();
    }
}