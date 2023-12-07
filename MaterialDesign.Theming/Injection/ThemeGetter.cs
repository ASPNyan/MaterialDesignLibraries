namespace MaterialDesign.Theming.Injection;

public class ThemeGetter
{
    public Theme Theme { get; private set; }

    private ThemeGetter(Theme theme)
    {
        Theme = theme;
    }

    public static ThemeGetter CreateFromTheme(Theme theme) => new(theme);
    public static async Task<ThemeGetter> CreateFromThemeSource(IThemeSource themeSource) => 
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