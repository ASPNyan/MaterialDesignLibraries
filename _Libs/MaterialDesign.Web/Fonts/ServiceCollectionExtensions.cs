using Microsoft.Extensions.DependencyInjection;

namespace MaterialDesign.Web.Fonts;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddFontCollection(this IServiceCollection serviceCollection) =>
        serviceCollection.AddScoped<FontFaceCollection>();
}