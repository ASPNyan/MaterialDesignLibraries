using MaterialDesign.Icons.Common.Components;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Icons;

public static class MaterialIconConfigExtensions
{
    private static IServiceCollection AddMaterialConfig(this IServiceCollection serviceCollection)
    {
        DynamicComponentOutlet.AddComponentSource<MdConfigurationHeadContent>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddDynamicMaterialIcons(this WebApplicationBuilder builder) => 
        builder.Services.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());

    public static IServiceCollection AddStaticMaterialIcons(this WebApplicationBuilder builder) => 
        builder.Services.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());
}