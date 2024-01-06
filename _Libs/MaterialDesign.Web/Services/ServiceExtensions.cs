using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Web.Services;

public static class ServiceExtensions
{
    /// <summary>
    /// Adds <see cref="DynamicHeadStorage"/> to the <see cref="IServiceProvider"/>.
    /// </summary>
    public static IServiceCollection AddDynamicHeadStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<DynamicHeadStorage>();
    
    /// <summary>
    /// Adds <see cref="DynamicComponentStorage"/> to the <see cref="IServiceProvider"/>.
    /// </summary>
    public static IServiceCollection AddDynamicComponentStorage(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<DynamicComponentStorage>();
}