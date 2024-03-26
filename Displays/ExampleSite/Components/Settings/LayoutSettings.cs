using MudBlazor;

namespace ExampleSite.Components.Settings;

public class LayoutSettings
{
    private DrawerPosition _drawerPosition = DrawerPosition.Left;
    private bool _closeDrawerOnMainFocus = true;
    private bool _closeDrawerOnNavigate = true;

    public required bool CloseDrawerOnNavigate
    {
        get => _closeDrawerOnNavigate;
        set
        {
            _closeDrawerOnNavigate = value;
            OnUpdate?.Invoke();
        }
    }

    public required bool CloseDrawerOnMainFocus
    {
        get => _closeDrawerOnMainFocus;
        set
        {
            _closeDrawerOnMainFocus = value;
            OnUpdate?.Invoke();
        }
    }

    public required DrawerPosition DrawerPosition
    {
        get => _drawerPosition;
        set
        {
            _drawerPosition = value;
            OnUpdate?.Invoke();
        }
    }

    public Anchor MudDrawerPosition => (Anchor)DrawerPosition;

    public event Action? OnUpdate;
}

public enum DrawerPosition
{
    Left = Anchor.Left,
    Right = Anchor.Right,
    Top = Anchor.Top,
    Bottom = Anchor.Bottom
}