namespace MusicApp.Services;

public class ScopedSettings
{
    public int FontWeight { get; private set; } = 400;

    public string CurrentFontClass { get; private set; } = "Roboto";
    public bool DynamicTheme { get; private set; } = true;

    public void ToggleDynamicTheme()
    {
        DynamicTheme = !DynamicTheme;
        
        OnThemeUpdate?.Invoke();
    }

    public void SwitchFontClass(string className)
    {
        CurrentFontClass = className;
        OnFontUpdate?.Invoke();
    }

    public void UpdateFontWeight(int value)
    {
        FontWeight = value;
        OnFontUpdate?.Invoke();
    }

    public event Action? OnThemeUpdate;
    public event Action? OnFontUpdate;
}