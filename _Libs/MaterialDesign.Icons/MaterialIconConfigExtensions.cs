using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MaterialDesign.Web.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace MaterialDesign.Icons;

/// <summary>
/// Extension methods for adding Material Icons to web projects. Server (now Web App) projects should use
/// methods ending in `ToWebApplication`, whereas WASM projects should use methods ending in `ToWebAssembly`.
/// <br/><br/>
/// Please don't forget to add <see cref="DynamicHeadOutlet"/> to your App.razor in server apps.
/// This is done in WASM apps automatically.
/// </summary>
public static class MaterialIconConfigExtensions
{
    private static IServiceCollection AddMaterialConfig(this IServiceCollection serviceCollection)
    {
        DynamicHeadOutlet.AddComponentSource<MdConfigurationHeadContent>();
        return serviceCollection;
    }
    
    /// <summary>
    /// Adds Material Icons to a Blazor Web App with support for Dynamic sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    public static IServiceCollection AddDynamicMaterialIconsToWebApplication(this IServiceCollection services) =>
        services.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());

    /// <summary>
    /// Adds Material Icons to a Blazor Web App with only support for Static sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    public static IServiceCollection AddStaticMaterialIconsToWebApplication(this IServiceCollection services) => 
        services.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());
    
    private static IServiceCollection AddDynamicHeadContentOutlet(this WebAssemblyHostBuilder builder)
    {
        _ = builder.RootComponents.Any(mapping => mapping.Selector is "head::after" 
            ? throw new Exception($"Root Component with type '{mapping.ComponentType.FullName}' has already " +
                                  $"taken selector 'head::after'. Please remove this to support the " +
                                  $"DynamicHeadOutlet. The HeadOutlet component is added as a source to the " +
                                  $"DynamicHeadOutlet automatically.")
            : false);
        
        builder.RootComponents.Add<DynamicHeadOutlet>("head::after");
        DynamicHeadOutlet.AddComponentSource<HeadOutlet>();
        return builder.Services.AddMaterialConfig();
    }

    /// <summary>
    /// Adds Material Icons to a Blazor WASM project with support for Dynamic sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddStaticMaterialIconsToWebAssembly(this WebAssemblyHostBuilder builder) => 
        builder.AddDynamicHeadContentOutlet().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());

    /// <summary>
    /// Adds Material Icons to a Blazor WASM project with only support for Static sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddDynamicMaterialIconsToWebAssembly(this WebAssemblyHostBuilder builder) => 
        builder.AddDynamicHeadContentOutlet().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());
}