namespace MaterialDesign.Theming.Injection.ThemeSources;

public class PresetColorSource : IThemeSource
{
    public HCTA Source { get; private set; } = new(0, 0, 0);

    public void WithSource(HCTA source) => Source = source;
    
    public Task<HCTA> GetSource() => new(() => Source);
}