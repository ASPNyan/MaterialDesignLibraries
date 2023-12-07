using MaterialDesign.Theming.Injection.ThemeSources;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddThemeService(this IServiceCollection serviceCollection, Theme theme) => 
        serviceCollection.AddScoped<ThemeGetter>(_ => ThemeGetter.CreateFromTheme(theme));

    public static IServiceCollection AddThemeService(this IServiceCollection serviceCollection,
        Func<IServiceProvider, Theme> builderMethod) =>
            serviceCollection.AddScoped<ThemeGetter>(serviceProvider =>
                ThemeGetter.CreateFromTheme(builderMethod(serviceProvider)));
    
    public static async Task<IServiceCollection> AddThemeService(this IServiceCollection serviceCollection,
        IThemeSource themeSource)
    {
        ThemeGetter getter = await ThemeGetter.CreateFromThemeSource(themeSource);
        return serviceCollection.AddScoped<ThemeGetter>(_ => getter);
    }

    public static async Task<IServiceCollection> AddThemeService(this IServiceCollection serviceCollection,
        Func<ThemeSourceBuilder, IThemeSource> builderMethod)
    {
        ThemeGetter getter = await ThemeGetter.CreateFromThemeSource(builderMethod(new ThemeSourceBuilder()));
        return serviceCollection.AddScoped<ThemeGetter>(_ => getter);
    }

    public static Task<IServiceCollection> AddThemeService(this IServiceCollection serviceCollection,
        Func<ThemeSourceBuilder, IServiceProvider, IThemeSource> builderMethod)
    {
        return Task.Run(() => serviceCollection.AddScoped<ThemeGetter>(serviceProvider =>
        {
            ThemeSourceBuilder builder = new ThemeSourceBuilder();
            IThemeSource themeSource = builderMethod(builder, serviceProvider);
            ThemeGetter getter = Task.Run(() => ThemeGetter.CreateFromThemeSource(themeSource)).Result;
            return getter;
        }));
    }
}