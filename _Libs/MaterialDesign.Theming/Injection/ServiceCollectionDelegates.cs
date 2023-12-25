using MaterialDesign.Theming.Injection.ThemeSources;

namespace MaterialDesign.Theming.Injection;

public delegate Theme ThemeFromServiceProvider(IServiceProvider serviceProvider);
public delegate Task<IThemeSource> ThemeSourceFromBuilder(ThemeSourceBuilder builder);
public delegate Task<IThemeSource> ThemeSourceFromBuilderAndServiceProvider(ThemeSourceBuilder builder, 
    IServiceProvider serviceProvider);