namespace MaterialDesign.Theming.Injection;

/// <summary>
/// A container class used to wrap a <see cref="Theming.Theme"/>, used in dependency injection. Supports <see cref="IThemeSource"/>s
/// </summary>
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

    /// <summary>
    /// Creates a new ThemeContainer from a <see cref="Theming.Theme"/>
    /// </summary>
    public static ThemeContainer CreateFromTheme(Theme theme) => new(theme);
    
    /// <summary>
    /// Creates a new ThemeContainer from a <see cref="IThemeSource"/> after converting it to a <see cref="Theming.Theme"/>.
    /// </summary>
    /// <param name="themeSource"></param>
    /// <returns></returns>
    public static async Task<ThemeContainer> CreateFromThemeSource(IThemeSource themeSource) => 
        new(await themeSource.GetTheme()); 
    
    /// <summary>
    /// Updates the ThemeContainer's <see cref="Theme"/>, calling <see cref="OnThemeUpdate"/> after.
    /// </summary>
    /// <param name="newTheme">The new, latest and greatest, theme.</param>
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
    
    /// <summary>
    /// Updates the ThemeContainer's <see cref="Theme"/> from an <see cref="IThemeSource"/>,
    /// calling <see cref="OnThemeUpdate"/> after.
    /// </summary>
    /// <param name="themeSource">The new, latest and greatest, theme (after it's been sourced).</param>
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
    
    // ReSharper disable InvalidXmlDocComment
    /// <summary>
    /// An event invoked when the current Theme is updated. This can occur either through <see cref="UpdateTheme"/> or
    /// Theme's <see cref="Theme.Update"/> methods, changing color modes (dark/light), etc. It is not recommended to
    /// subscribe to Theme's <see cref="Theme.OnUpdate"/> event because <see cref="UpdateTheme"/> replaces the actual
    /// class itself, not just using <see cref="Theme.Update"/>
    /// </summary>
    // ReSharper restore InvalidXmlDocComment
    public event Action? OnThemeUpdate;
}