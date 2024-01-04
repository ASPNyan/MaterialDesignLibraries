namespace MusicApp.Services;

public class DynamicThemeSettings
{
    public bool DynamicTheme { get; private set; } = true;

    public void ToggleDynamicTheme()
    {
        DynamicTheme = !DynamicTheme;
        
        OnUpdate?.Invoke();
    }

    public event Action? OnUpdate;
}