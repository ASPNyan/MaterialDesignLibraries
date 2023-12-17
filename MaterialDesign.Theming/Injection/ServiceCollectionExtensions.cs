using MaterialDesign.Theming.Injection.ThemeSources;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Theming.Injection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection, Theme theme)
    {
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        return serviceCollection.AddScoped<ThemeContainer>(_ => ThemeContainer.CreateFromTheme(theme));
    }

    public static IServiceCollection AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeFromServiceProvider builderMethod)
    {
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        return serviceCollection.AddScoped<ThemeContainer>(serviceProvider =>
            ThemeContainer.CreateFromTheme(builderMethod(serviceProvider)));
    }

    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        IThemeSource themeSource)
    {
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        ThemeContainer container = await ThemeContainer.CreateFromThemeSource(themeSource);
        return serviceCollection.AddScoped<ThemeContainer>(_ => container);
    }

    public static async Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeSourceFromBuilder builderMethod)
    {
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        ThemeContainer container = await ThemeContainer.CreateFromThemeSource(builderMethod(new ThemeSourceBuilder()));
        return serviceCollection.AddScoped<ThemeContainer>(_ => container);
    }

    public static Task<IServiceCollection> AddMaterialThemeService(this IServiceCollection serviceCollection,
        ThemeSourceFromBuilderAndServiceProvider builderMethod)
    {
        serviceCollection.AddScoped<Theme>(sp => sp.GetRequiredService<ThemeContainer>().Theme);
        return new Task<IServiceCollection>(() => serviceCollection.AddScoped<ThemeContainer>(serviceProvider =>
        {
            ThemeSourceBuilder builder = new ThemeSourceBuilder();
            IThemeSource themeSource = builderMethod(builder, serviceProvider);
            ThemeContainer container = Task.Run(() => ThemeContainer.CreateFromThemeSource(themeSource)).Result;
            return container;
        }));
    }
}