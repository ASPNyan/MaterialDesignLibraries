using MaterialDesign.Theming.Injection.ThemeSources;

namespace MaterialDesign.Theming.Injection;

public interface IThemeSource
{
    protected virtual Theme? UseActualTheme => null;
    
    protected HCTA GetSourceSynchronous() => GetSource().Result;

    protected Task<HCTA> GetSource()
    {
        throw new InvalidOperationException($"{GetType().Name}, not marked with UseActualTheme, has not implemented " +
                                            $"{nameof(GetSource)}. Either implement it or set UseActualTheme");
    }

    public async Task<Theme> GetTheme()
    {
        return UseActualTheme ?? new Theme(await GetSource());
    }

    public static sealed ThemeSourceBuilder Builder => new();
}