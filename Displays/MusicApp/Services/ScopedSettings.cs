namespace MusicApp.Services;

public class ScopedSettings
{
    public string CurrentFontClass { get; set; } = "Roboto";
    public bool DynamicTheme { get; private set; } = true;

    public void ToggleDynamicTheme()
    {
        DynamicTheme = !DynamicTheme;
        
        OnUpdate?.Invoke();
    }

    public event Action? OnUpdate;
}