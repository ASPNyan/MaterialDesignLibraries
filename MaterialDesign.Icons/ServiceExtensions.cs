using MaterialDesign.Icons.Common.Components;
using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Icons;

public static class ServiceCollectionExtensions
{
    private static IServiceCollection AddMaterialConfig(this IServiceCollection serviceCollection)
    {
        DynamicHeadOutlet.AddComponentSource<MdConfigurationHeadContent>();
        return serviceCollection;
    }
    
    public static IServiceCollection AddDynamicMaterialIcons(this IServiceCollection serviceCollection) => 
        serviceCollection.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateDynamic());

    public static IServiceCollection AddStaticMaterialIcons(this IServiceCollection serviceCollection) => 
        serviceCollection.AddMaterialConfig().AddSingleton<MdIconConfiguration>(_ => MdIconConfiguration.CreateStatic());
}