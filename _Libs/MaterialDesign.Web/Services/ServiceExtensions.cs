using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Web.Services;

public static class ServiceExtensions
{
    public static IServiceCollection AddDynamicHeadStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<DynamicHeadStorage>();
    
    public static IServiceCollection AddDynamicComponentStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<DynamicComponentStorage>();
}