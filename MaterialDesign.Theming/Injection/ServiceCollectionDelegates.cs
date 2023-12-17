using MaterialDesign.Theming.Injection.ThemeSources;

namespace MaterialDesign.Theming.Injection;

public delegate Theme ThemeFromServiceProvider(IServiceProvider serviceProvider);
public delegate IThemeSource ThemeSourceFromBuilder(ThemeSourceBuilder builder);
public delegate IThemeSource ThemeSourceFromBuilderAndServiceProvider(ThemeSourceBuilder builder, 
    IServiceProvider serviceProvider);