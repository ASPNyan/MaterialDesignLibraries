namespace MaterialDesign.Theming.Injection;

/// <summary>
/// A container class used to wrap a <see cref="IScheme"/>, used in dependency injection. Supports <see cref="IThemeSource"/>s.
/// </summary>
public class ThemeContainer // Update to SchemeContainer in next major.
{
    [Obsolete("Please use Scheme instead of Theme, as Theme will be removed in the next major update.")]
    public Theme Theme => Scheme as Theme ?? throw new InvalidOperationException(
        "Current scheme is not a theme, but an alternative IScheme-implementing class.");
    public IScheme Scheme { get; private set; }
    
    private ThemeContainer(IScheme theme)
    {
        Scheme = theme;
        SubscribeToEvent();
    }


    /// <summary>
    /// Creates a new ThemeContainer from a <see cref="Theming.Theme"/>
    /// </summary>
    [Obsolete("Please switch to CreateFromScheme instead of CreateFromTheme")]
    public static ThemeContainer CreateFromTheme(Theme theme) => new(theme);
    
    /// <summary>
    /// Creates a new ThemeContainer from a <see cref="Theming.IScheme"/>
    /// </summary>
    public static ThemeContainer CreateFromScheme(IScheme scheme) => new(scheme);
    
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
    [Obsolete("Please switch to UpdateScheme instead of UpdateTheme")]
    public void UpdateTheme(Theme newTheme)
    {
        UnsubscribeFromEvent();
        
        bool wasDark = Scheme?.IsDarkScheme ?? false;
        Scheme = newTheme;
        if (wasDark) Scheme.SetDark();
        else Scheme.SetLight();
        
        SubscribeToEvent();
        OnSchemeUpdate?.Invoke();
    }
    
    /// <summary>
    /// Updates the ThemeContainer's <see cref="Scheme"/>, calling <see cref="OnThemeUpdate"/> after.
    /// </summary>
    /// <param name="newScheme">The new, latest and greatest, theme.</param>
    public void UpdateScheme(IScheme newScheme)
    {
        UnsubscribeFromEvent();
        
        bool wasDark = Scheme?.IsDarkScheme ?? false;
        Scheme = newScheme;
        if (wasDark) Scheme.SetDark();
        else Scheme.SetLight();
        
        SubscribeToEvent();
        OnSchemeUpdate?.Invoke();
    }
    
    /// <summary>
    /// Updates the ThemeContainer's <see cref="Theme"/> from an <see cref="IThemeSource"/>,
    /// calling <see cref="OnThemeUpdate"/> after.
    /// </summary>
    /// <param name="themeSource">The new, latest and greatest, theme (after it's been sourced).</param>
    public async Task UpdateThemeSource(IThemeSource themeSource)
    {
        UnsubscribeFromEvent();
        
        bool wasDark = Scheme?.IsDarkScheme ?? false;
        Scheme = await themeSource.GetTheme();
        if (wasDark) Scheme.SetDark();
        else Scheme.SetLight();
        
        SubscribeToEvent();
        OnSchemeUpdate?.Invoke();
    }

    
    private void SchemeUpdate() => OnSchemeUpdate?.Invoke();

    // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    private void SubscribeToEvent()
    {
        if (Scheme is not null) Scheme.OnUpdate += SchemeUpdate;
    }

    private void UnsubscribeFromEvent()
    {
        if (Scheme is not null) Scheme.OnUpdate -= SchemeUpdate;
    }
    // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
    
    /// <summary>
    /// An event invoked when the current Theme is updated. This can occur either through <see cref="UpdateTheme"/> or
    /// Theme's Update methods, changing color modes (dark/light), etc. It is not recommended to
    /// subscribe to Theme's <see cref="Theme.OnUpdate"/> event because <see cref="UpdateTheme"/> replaces the actual
    /// class itself, not just by calling Update.
    /// </summary>
    [Obsolete("Please use OnSchemeUpdate instead of OnThemeUpdate")]
    public event Action? OnThemeUpdate
    {
        add => OnSchemeUpdate += value;
        remove => OnSchemeUpdate -= value;
    }

    /// <summary>
    /// An event invoked when the current Theme is updated. This can occur either through <see cref="UpdateTheme"/>,
    /// changing color modes (dark/light), etc. It is not recommended to subscribe to Scheme's
    /// <see cref="IScheme.OnUpdate"/> event because <see cref="UpdateTheme"/> replaces the actual class itself,
    /// not just by calling Update.
    /// </summary>
    public event Action? OnSchemeUpdate;
}