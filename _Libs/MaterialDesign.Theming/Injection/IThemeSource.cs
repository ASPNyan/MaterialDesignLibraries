namespace MaterialDesign.Theming.Injection;

/// <summary>
/// Provides ways to asynchronously create <see cref="Theme"/>s when required in methods and classes like <see cref="ThemeContainer"/>.
/// </summary>
public interface IThemeSource
{
    /// <summary>
    /// Set to <see langword="this"/> (<see langword="Me"/> in VB) in <see cref="Theme"/>, otherwise it should be null
    /// unless it is set to a different <see cref="Theme"/> instance. <see cref="GetSource"/> is not used when
    /// this is set, as it will use this instead.
    /// </summary>
    protected Theme? UseActualTheme => null;
    
    /// <summary>
    /// Gets a color synchronously from the source. Unsupported in WASM as <see cref="Monitor.Wait"/> calls will crash the app.
    /// </summary>
    protected HCTA GetSourceSynchronous() => GetSource().Result;

    /// <summary>
    /// Gets the source color of the theme asynchronously. Not required to be overridden when <see cref="UseActualTheme"/> is set.
    /// </summary>
    protected Task<HCTA> GetSource()
    {
        throw new InvalidOperationException($"{GetType().Name}, not marked with UseActualTheme, has not implemented " +
                                            $"{nameof(GetSource)}. Either implement it or set UseActualTheme");
    }

    /// <summary>
    /// Gets the theme asynchronously from the supplied theme source.
    /// </summary>
    /// <returns>The sourced <see cref="Theme"/></returns>
    public async Task<Theme> GetTheme()
    {
        return UseActualTheme ?? new Theme(await GetSource());
    }
}