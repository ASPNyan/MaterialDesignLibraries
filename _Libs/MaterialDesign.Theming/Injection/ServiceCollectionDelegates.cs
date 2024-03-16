using MaterialDesign.Theming.Injection.ThemeSources;

namespace MaterialDesign.Theming.Injection;

/// <summary>
/// Creates a theme synchronously, optionally using the provided <see cref="IServiceProvider"/>.
/// </summary>
[Obsolete("Obsolete due to ThemeContainer.Theme, use ISchemeFromServiceProvider instead.")]
public delegate Theme ThemeFromServiceProvider(IServiceProvider serviceProvider);

/// <summary>
/// Creates a scheme synchronously, optionally using the provided <see cref="IServiceProvider"/>.
/// </summary>
public delegate IScheme SchemeFromServiceProvider(IServiceProvider serviceProvider);

/// <summary>
/// Creates a theme asynchronously, using the provided <see cref="ThemeSourceBuilder"/> and building it at the end.
/// </summary>
public delegate Task<IThemeSource> ThemeSourceFromBuilder(ThemeSourceBuilder builder);

/// <summary>
/// Creates a theme asynchronously, optionally using the provided <see cref="IServiceProvider"/> and using the provided
/// <see cref="ThemeSourceBuilder"/> and building it at the end.
/// </summary>
public delegate Task<IThemeSource> ThemeSourceFromBuilderAndServiceProvider(ThemeSourceBuilder builder, 
    IServiceProvider serviceProvider);