using MaterialDesign.Theming.Injection.ThemeSources;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

/// <summary>
/// A static class containing extensions for <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    internal static bool CheckSetFail;
    
    private static bool _themeServiceSet;

    // The theme service shouldn't even be a thing honestly. There will be no replacement of it. Remove in next major.
    [Obsolete("Obsolete due to ThemeContainer.Theme")]
    private static void SetThemeService(IServiceCollection serviceCollection)
    {
        if (_themeServiceSet) return;
        
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        _themeServiceSet = true;
    }
    
    /// <summary>
    /// Adds Material Theming services to Dependency Injection through the <see cref="IServiceCollection"/>
    /// by creating a <see cref="ThemeContainer"/> with the supplied <paramref name="theme"/>.
    /// </summary>
    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection, Theme theme)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SetThemeService(serviceCollection);
#pragma warning restore CS0618 // Type or member is obsolete
        return serviceCollection.AddScoped<ThemeContainer>(_ => ThemeContainer.CreateFromScheme(theme));
    }

    /// <summary>
    /// Adds Material Theming services to Dependency Injection through the <see cref="IServiceCollection"/>
    /// by creating a <see cref="ThemeContainer"/> with the supplied <paramref name="scheme"/>.
    /// </summary>
    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection, IScheme scheme) => 
        serviceCollection.AddScoped<ThemeContainer>(_ => ThemeContainer.CreateFromScheme(scheme));

    /// <summary>
    /// Adds Material Theming services to Dependency Injection through the <see cref="IServiceCollection"/>
    /// by creating a <see cref="ThemeContainer"/> with the supplied <see cref="IThemeSource"/> <paramref name="themeSource"/>.
    /// </summary>
    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        IThemeSource themeSource)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SetThemeService(serviceCollection);
#pragma warning restore CS0618 // Type or member is obsolete
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

    /// <summary>
    /// Adds Material Theming services to Dependency Injection through the <see cref="IServiceCollection"/>
    /// by creating a <see cref="ThemeContainer"/> when the service is requested through the <see cref="IServiceProvider"/>,
    /// with the <see cref="IServiceProvider"/> as a parameter or using the provided <paramref name="fallback"/> if that fails.
    /// </summary>
    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeFromServiceProvider builderMethod, Theme? fallback = null)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SetThemeService(serviceCollection);
#pragma warning restore CS0618 // Type or member is obsolete
        return serviceCollection.AddScoped<ThemeContainer>(serviceProvider =>
            ThemeContainer.CreateFromScheme(TryWithFallback(() => builderMethod(serviceProvider), fallback)));
    }

    /// <summary>
    /// Adds Material Theming services to Dependency Injection through the <see cref="IServiceCollection"/>
    /// by creating a <see cref="ThemeContainer"/> when the service is requested through the <see cref="IServiceProvider"/>,
    /// with a <see cref="ThemeSourceBuilder"/> as a parameter or using the provided <paramref name="fallback"/> if that fails.
    /// </summary>
    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeSourceFromBuilder builderMethod, IThemeSource? fallback = null)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SetThemeService(serviceCollection);
#pragma warning restore CS0618 // Type or member is obsolete
        ThemeContainer container = await ThemeContainer.CreateFromThemeSource(await TryWithFallback(
            () => builderMethod(new ThemeSourceBuilder()), fallback));
        return serviceCollection.AddScoped<ThemeContainer>(_ => container);
    }

    /// <summary>
    /// Adds a null <see cref="Theme"/> to a <see cref="ThemeContainer"/> service. Requires
    /// <see cref="SetMaterialThemeService"/> be called after. This method should not be used outside Blazor WASM as
    /// it uses singleton instead of scoped because WASM apps have no differences between singleton and scoped, but
    /// scoped services cannot be accessed without a scope.
    /// </summary>
    public static void AddMaterialThemeService(this IServiceCollection serviceCollection)
    {
#pragma warning disable CS0618 // Type or member is obsolete
        SetThemeService(serviceCollection);
#pragma warning restore CS0618 // Type or member is obsolete

        serviceCollection.AddSingleton<ThemeContainer>(_ => ThemeContainer.CreateFromScheme(null!));

        CheckSetFail = true;
    }

    /// <summary>
    /// Sets the <see cref="ThemeContainer"/> service outside of the app (and therefore off of the render thread) so that
    /// <see cref="IThemeSource"/>s can be used without blocking the render thread. Defaults to the provided
    /// <paramref name="fallback"/> as required.
    /// </summary>
    public static async Task SetMaterialThemeService(this WebAssemblyHost host,
        ThemeSourceFromBuilderAndServiceProvider builderMethod, IThemeSource? fallback)
    {
        IServiceProvider serviceProvider = host.Services;
        
        ThemeContainer result = await ThemeContainer.CreateFromThemeSource(
                await TryWithFallback(() => builderMethod(new ThemeSourceBuilder(), serviceProvider), fallback));
        
        serviceProvider.GetRequiredService<ThemeContainer>().UpdateScheme(result.Scheme);

        CheckSetFail = false;
    }
}