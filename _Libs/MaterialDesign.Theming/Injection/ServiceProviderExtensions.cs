using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

/// <summary>
/// A static class containing extensions for <see cref="IServiceProvider"/>.
/// </summary>
public static class ServiceProviderExtensions
{
    /// <summary>
    /// Sets the default theme scheme mode, (i.e. dark mode, light mode (political, but ew))
    /// </summary>
    public static async Task SetDefaultThemeMode(this IServiceProvider serviceProvider, bool defaultIsDark)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var container = scope.ServiceProvider.GetRequiredService<ThemeContainer>();
        
        if (defaultIsDark) container.Theme.SetDark();
        else container.Theme.SetLight();
    }
}