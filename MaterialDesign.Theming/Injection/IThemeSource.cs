using MaterialDesign.Theming.Injection.ThemeSources;

namespace MaterialDesign.Theming.Injection;

public interface IThemeSource
{
    public HCTA GetSourceSynchronous() => GetSource().Result;

    public Task<HCTA> GetSource();

    public static sealed ThemeSourceBuilder Builder => new();
}