using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using MaterialDesign.Web.Components;
using MaterialDesign.Web.Services;
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
    public delegate string UrlGenerator(MdIconLineStyle style);
    
    private static IServiceCollection AddMaterialConfig(this IServiceCollection serviceCollection,
        UrlGenerator? urlGenerator)
    {
#nullable disable
        Dictionary<string, object> param = new() { {nameof(MdConfigurationHeadContent.UrlGenerator), urlGenerator} };
        DynamicHeadOutlet.AddComponentSource<MdConfigurationHeadContent>(param);
#nullable restore
        return serviceCollection.AddFontCollection();
    }
    
    /// <summary>
    /// Adds Material Icons to a Blazor Web App with support for Dynamic sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> on the builder.</param>
    /// <param name="urlGenerator">
    /// A custom method to generate a url pointing to the font files that contain the material icon fonts.
    /// Leave null to use the fonts at <c>fonts.googleapis.com</c>. This should mainly be used to point to
    /// local font files for offline use.
    /// </param>
    public static IServiceCollection AddDynamicMaterialIconsToWebApplication(this IServiceCollection services,
        UrlGenerator? urlGenerator = null) =>
        services.AddMaterialConfig(urlGenerator).AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());

    /// <summary>
    /// Adds Material Icons to a Blazor Web App with only support for Static sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    /// <inheritdoc cref="AddDynamicMaterialIconsToWebApplication"/>
    public static IServiceCollection AddStaticMaterialIconsToWebApplication(this IServiceCollection services,
        UrlGenerator? urlGenerator = null) => 
        services.AddMaterialConfig(urlGenerator).AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());
    
    private static IServiceCollection AddDynamicHeadContentOutlet(this WebAssemblyHostBuilder builder, 
        UrlGenerator? urlGenerator)
    {
        _ = builder.RootComponents.Any(mapping => mapping.Selector is "head::after" 
            ? throw new Exception($"Root Component with type '{mapping.ComponentType.FullName}' has already " +
                                  $"taken selector 'head::after'. Please remove this to support the " +
                                  $"DynamicHeadOutlet. The HeadOutlet component is added as a source to the " +
                                  $"DynamicHeadOutlet automatically.")
            : false);
        
        builder.RootComponents.Add<DynamicHeadOutlet>("head::after");
        DynamicHeadOutlet.AddComponentSource<HeadOutlet>();
        return builder.Services.AddMaterialConfig(urlGenerator);
    }

    /// <summary>
    /// Adds Material Icons to a Blazor WASM project with only support for Static sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    /// <param name="builder">The current <see cref="WebAssemblyHostBuilder"/>.</param>
    /// <param name="urlGenerator">
    /// A custom method to generate a url pointing to the font files that contain the material icon fonts.
    /// Leave null to use the fonts at <c>fonts.googleapis.com</c>. This should mainly be used to point to
    /// local font files for offline use.
    /// </param>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddStaticMaterialIconsToWebAssembly(this WebAssemblyHostBuilder builder,
        UrlGenerator? urlGenerator = null) => 
        builder.AddDynamicHeadContentOutlet(urlGenerator).AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());

    /// <summary>
    /// Adds Material Icons to a Blazor WASM project with support for Dynamic sizing. More info at
    /// https://fonts.google.com/icons
    /// </summary>
    /// <inheritdoc cref="AddStaticMaterialIconsToWebAssembly"/>
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddDynamicMaterialIconsToWebAssembly(this WebAssemblyHostBuilder builder,
        UrlGenerator? urlGenerator = null) => 
        builder.AddDynamicHeadContentOutlet(urlGenerator).AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());
}