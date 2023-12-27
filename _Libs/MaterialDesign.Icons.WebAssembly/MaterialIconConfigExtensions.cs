using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using MaterialDesign.Web.Components;

namespace MaterialDesign.Icons;

public static class MaterialIconConfigExtensions
{
    private static IServiceCollection AddMaterialConfig(this IServiceCollection serviceCollection)
    {
        DynamicHeadOutlet.AddComponentSource<MdConfigurationHeadContent>();
        return serviceCollection;
    }

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

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddStaticMaterialIcons(this WebAssemblyHostBuilder builder) => 
        builder.AddDynamicHeadContentOutlet().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());

    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(DynamicHeadOutlet))]
    public static IServiceCollection AddDynamicMaterialIcons(this WebAssemblyHostBuilder builder) => 
        builder.AddDynamicHeadContentOutlet().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());
}