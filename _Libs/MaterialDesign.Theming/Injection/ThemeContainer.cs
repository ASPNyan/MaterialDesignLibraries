namespace MaterialDesign.Theming.Injection;

public class ThemeContainer
{
    public Theme Theme { get; private set; }

    private ThemeContainer(Theme theme)
    {
        Theme = theme;
    }

    public static ThemeContainer CreateFromTheme(Theme theme) => new(theme);
    public static async Task<ThemeContainer> CreateFromThemeSource(IThemeSource themeSource) => 
        new(new Theme(await themeSource.GetSource())); 
    
    public void UpdateTheme(Theme newTheme)
    {
        Theme = newTheme;
        OnThemeUpdate?.Invoke();
    }
    
    public async Task UpdateThemeSource(IThemeSource themeSource)
    {
        Theme = new Theme(await themeSource.GetSource());
        OnThemeUpdate?.Invoke();
    }

    public event Action? OnThemeUpdate;
}