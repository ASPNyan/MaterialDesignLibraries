using MaterialDesign.Theming.Injection.ThemeSources;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

public static class ServiceCollectionExtensions
{
    internal static bool CheckSetFail;
    
    private static bool _themeServiceSet;

    private static void SetThemeService(IServiceCollection serviceCollection)
    {
        if (_themeServiceSet) return;
        
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        _themeServiceSet = true;
    }
    
    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection, Theme theme)
    {
        SetThemeService(serviceCollection);
        return serviceCollection.AddScoped<ThemeContainer>(_ => ThemeContainer.CreateFromTheme(theme));
    }
    
    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        IThemeSource themeSource)
    {
        SetThemeService(serviceCollection);
        ThemeContainer container = await ThemeContainer.CreateFromThemeSource(themeSource);
        return serviceCollection.AddScoped<ThemeContainer>(_ => container);
    }

    private static T TryWithFallback<T>(Func<T> builderClosure, T? fallback) where T : notnull
    {
        T value;
        
        try
        {
            value = builderClosure();
        }
        catch 
        {
            if (fallback is null) throw;
            value = fallback;
        }

        return value;
    }
    
    private static async Task<T> TryWithFallback<T>(Func<Task<T>> builderClosure, T? fallback) where T : notnull
    {
        T value;
        
        try
        {
            value = await builderClosure();
        }
        catch 
        {
            if (fallback is null) throw;
            value = fallback;
        }

        return value;
    }

    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeFromServiceProvider builderMethod, Theme? fallback = null)
    {
        SetThemeService(serviceCollection);
        return serviceCollection.AddScoped<ThemeContainer>(serviceProvider =>
            ThemeContainer.CreateFromTheme(TryWithFallback(() => builderMethod(serviceProvider), fallback)));
    }

    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeSourceFromBuilder builderMethod, IThemeSource? fallback = null)
    {
        SetThemeService(serviceCollection);
        ThemeContainer container = await ThemeContainer.CreateFromThemeSource(await TryWithFallback(
            () => builderMethod(new ThemeSourceBuilder()), fallback));
        return serviceCollection.AddScoped<ThemeContainer>(_ => container);
    }

    public static void AddMaterialThemeService(this IServiceCollection serviceCollection)
    {
        SetThemeService(serviceCollection);

        serviceCollection.AddScoped<ThemeContainer>(_ => ThemeContainer.CreateFromTheme(null!));

        CheckSetFail = true;
    }

    public static async Task SetMaterialThemeService(this WebAssemblyHost host,
        ThemeSourceFromBuilderAndServiceProvider builderMethod, IThemeSource? fallback)
    {
        IServiceProvider serviceProvider = host.Services;
        
        ThemeContainer result = await ThemeContainer.CreateFromThemeSource(
                await TryWithFallback(() => builderMethod(new ThemeSourceBuilder(), serviceProvider), fallback));
        
        serviceProvider.GetRequiredService<ThemeContainer>().UpdateTheme(result.Theme);

        CheckSetFail = false;
    }
}