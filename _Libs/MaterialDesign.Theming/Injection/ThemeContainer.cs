namespace MaterialDesign.Theming.Injection;

public class ThemeContainer
{
    private Theme _theme;

    public Theme Theme
    {
        get => _theme;
        private set => _theme = value;
    }

    private static void CheckCreation()
    {
        if (ServiceCollectionExtensions.CheckSetFail)
            throw new Exception("ThemeContainer cannot be used when SetMaterialThemeService was not called.");
    }

    private ThemeContainer(Theme theme)
    {
        _theme = theme;
        SubscribeToEvent();
    }

    public static ThemeContainer CreateFromTheme(Theme theme) => new(theme);
    public static async Task<ThemeContainer> CreateFromThemeSource(IThemeSource themeSource) => 
        new(await themeSource.GetTheme()); 
    
    public void UpdateTheme(Theme newTheme)
    {
        UnsubscribeFromEvent();
        
        bool wasDark = Theme?.IsDarkScheme ?? false;
        Theme = newTheme;
        if (wasDark) Theme.SetDark();
        else Theme.SetLight();
        
        SubscribeToEvent();
        OnThemeUpdate?.Invoke();
    }
    
    public async Task UpdateThemeSource(IThemeSource themeSource)
    {
        UnsubscribeFromEvent();
        
        bool wasDark = Theme?.IsDarkScheme ?? false;
        Theme = await themeSource.GetTheme();
        if (wasDark) Theme.SetDark();
        else Theme.SetLight();
        
        SubscribeToEvent();
        OnThemeUpdate?.Invoke();
    }

    private void ThemeUpdate() => OnThemeUpdate?.Invoke();

    // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    private void SubscribeToEvent()
    {
        if (Theme is not null) Theme.OnUpdate += ThemeUpdate;
    }

    private void UnsubscribeFromEvent()
    {
        if (Theme is not null) Theme.OnUpdate -= ThemeUpdate;
    }
    // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract

    public event Action? OnThemeUpdate;
}