using System.Diagnostics.Contracts;
using MudBlazor;

namespace ExampleSite.Components.Settings;

public class LayoutSettings : IEquatable<LayoutSettings>
{
    private Guid Hash { get; } = Guid.NewGuid();
    
    private LayoutPosition _drawerPosition = LayoutPosition.Left;
    private LayoutPosition _headerPosition = LayoutPosition.Top;
    private Color _layoutColor = Color.Secondary;
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

    public required LayoutPosition DrawerPosition
    {
        get => _drawerPosition;
        set
        {
            if (value is not LayoutPosition.Left and not LayoutPosition.Right)
                throw new ArgumentOutOfRangeException(nameof(value), value, "DrawerPosition must be ether Left or Right");
            
            _drawerPosition = value;
            OnUpdate?.Invoke();
        }
    }

    public required LayoutPosition HeaderPosition
    {
        get => _headerPosition;
        set
        {
            if (value is not LayoutPosition.Top and not LayoutPosition.Bottom)
                throw new ArgumentOutOfRangeException(nameof(value), value, "HeaderPosition must be ether Top or Bottom");
            
            _headerPosition = value;
            OnUpdate?.Invoke();
        }
    }

    public required Color LayoutColor
    {
        get => _layoutColor;
        set
        {
            _layoutColor = value;
            OnUpdate?.Invoke();
        }
    }

    public Anchor MudDrawerPosition => (Anchor)DrawerPosition;

    public string HeaderClass
    {
        get
        {
            switch (LayoutColor)
            {
                case Color.Primary:
                case Color.Secondary:
                case Color.Tertiary:
                    return $"{LayoutColor}-container on-{LayoutColor}-container-text".ToLowerInvariant();
                case Color.PrimarySwapped:
                case Color.SecondarySwapped:
                case Color.TertiarySwapped:
                    string color = LayoutColor.ToString().Replace("Swapped", null).ToLowerInvariant();
                    return $"on-{color} on-{color}-container-text";
                default:
                    throw new ArgumentOutOfRangeException(nameof(LayoutColor), LayoutColor, "The set LayoutColor was invalid.");
            }
        }
    }
    
    public string DrawerClass
    {
        get
        {
            switch (LayoutColor)
            {
                case Color.Primary:
                case Color.Secondary:
                case Color.Tertiary:
                    return $"on-{LayoutColor} on-{LayoutColor}-container-text".ToLowerInvariant();
                case Color.PrimarySwapped:
                case Color.SecondarySwapped:
                case Color.TertiarySwapped:
                    string color = LayoutColor.ToString().Replace("Swapped", null).ToLowerInvariant();
                    return $"{color}-container on-{color}-container-text";
                default:
                    throw new ArgumentOutOfRangeException(nameof(LayoutColor), LayoutColor, "The set LayoutColor was invalid.");
            }
        }
    }

    public void Update(LayoutSettings newSettings)
    {
        _drawerPosition = newSettings._drawerPosition;
        _headerPosition = newSettings._headerPosition;
        _layoutColor = newSettings._layoutColor;
        _closeDrawerOnMainFocus = newSettings._closeDrawerOnMainFocus;
        _closeDrawerOnNavigate = newSettings._closeDrawerOnNavigate;
        OnUpdate?.Invoke();
    }

    [Pure]
    public LayoutSettings Clone() => new()
    {
        CloseDrawerOnNavigate = CloseDrawerOnNavigate,
        CloseDrawerOnMainFocus = CloseDrawerOnMainFocus,
        DrawerPosition = DrawerPosition,
        HeaderPosition = HeaderPosition,
        LayoutColor = LayoutColor
    };

    public event Action? OnUpdate;
    
    public enum LayoutPosition
    {
        Left = Anchor.Left,
        Right = Anchor.Right,
        Top = Anchor.Top,
        Bottom = Anchor.Bottom
    }

    public enum Color
    {
        Primary,
        PrimarySwapped,
        Secondary,
        SecondarySwapped,
        Tertiary,
        TertiarySwapped,
    }

    public bool Equals(LayoutSettings? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return _drawerPosition == other._drawerPosition 
               && _headerPosition == other._headerPosition 
               && _layoutColor == other._layoutColor 
               && _closeDrawerOnMainFocus == other._closeDrawerOnMainFocus
               && _closeDrawerOnNavigate == other._closeDrawerOnNavigate;
    }

    public override bool Equals(object? obj) => obj is LayoutSettings settings && Equals(settings);

    public override int GetHashCode() => Hash.GetHashCode();


    public static bool operator ==(LayoutSettings? left, LayoutSettings? right) => Equals(left, right);

    public static bool operator !=(LayoutSettings? left, LayoutSettings? right) => !Equals(left, right);
}