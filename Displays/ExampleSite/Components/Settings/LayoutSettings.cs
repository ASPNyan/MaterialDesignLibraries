using System.Diagnostics.Contracts;
using MaterialDesign.Web.Fonts;
using MaterialDesign.Web.Fonts.Enums;
using MudBlazor;

namespace ExampleSite.Components.Settings;

public class LayoutSettings : IEquatable<LayoutSettings>
{
    private Guid Hash { get; } = Guid.NewGuid();
    
    private LayoutPosition _drawerPosition = LayoutPosition.Left;
    private LayoutPosition _headerPosition = LayoutPosition.Top;
    private Color _headerColor = Color.Secondary;
    private Color _drawerColor = Color.Secondary;
    private bool _closeDrawerOnMainFocus = true;
    private bool _closeDrawerOnNavigate = true;
    private bool _floatingDrawer = true;
    private FontFace _pageFont = Fonts.Roboto[FontWeightValue.Regular];

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
    
    public required bool FloatingDrawer
    {
        get => _floatingDrawer;
        set
        {
            _floatingDrawer = value;
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

    public required Color HeaderColor
    {
        get => _headerColor;
        set
        {
            _headerColor = value;
            OnUpdate?.Invoke();
        }
    }

    public required Color DrawerColor
    {
        get => _drawerColor;
        set
        {
            _drawerColor = value;
            OnUpdate?.Invoke();
        }
    }

    public required FontFace PageFont
    {
        get => _pageFont;
        set
        {
            _pageFont = value;
            OnUpdate?.Invoke();
        }
    }

    public Anchor MudDrawerPosition => (Anchor)DrawerPosition;

    public string HeaderClass
    {
        get
        {
            switch (HeaderColor)
            {
                case Color.Primary:
                case Color.Secondary:
                case Color.Tertiary:
                    return $"{HeaderColor}-container on-{HeaderColor}-container-text".ToLowerInvariant();
                case Color.PrimarySwapped:
                case Color.SecondarySwapped:
                case Color.TertiarySwapped:
                    string color = HeaderColor.ToString().Replace("Swapped", null).ToLowerInvariant();
                    return $"on-{color} on-{color}-container-text";
                default:
                    throw new ArgumentOutOfRangeException(nameof(HeaderColor), HeaderColor, "The set LayoutColor was invalid.");
            }
        }
    }
    
    public string DrawerClass
    {
        get
        {
            switch (DrawerColor)
            {
                case Color.Primary:
                case Color.Secondary:
                case Color.Tertiary:
                    return $"on-{DrawerColor} on-{DrawerColor}-container-text".ToLowerInvariant();
                case Color.PrimarySwapped:
                case Color.SecondarySwapped:
                case Color.TertiarySwapped:
                    string color = DrawerColor.ToString().Replace("Swapped", null).ToLowerInvariant();
                    return $"{color}-container on-{color}-container-text";
                default:
                    throw new ArgumentOutOfRangeException(nameof(DrawerColor), DrawerColor, "The set LayoutColor was invalid.");
            }
        }
    }

    public void Update(LayoutSettings newSettings)
    {
        _drawerPosition = newSettings._drawerPosition;
        _headerPosition = newSettings._headerPosition;
        _headerColor = newSettings._headerColor;
        _drawerColor = newSettings._drawerColor;
        _closeDrawerOnMainFocus = newSettings._closeDrawerOnMainFocus;
        _closeDrawerOnNavigate = newSettings._closeDrawerOnNavigate;
        _floatingDrawer = newSettings._floatingDrawer;
        _pageFont = newSettings._pageFont;
        OnUpdate?.Invoke();
    }

    [Pure]
    public LayoutSettings Clone() => new()
    {
        CloseDrawerOnNavigate = CloseDrawerOnNavigate,
        CloseDrawerOnMainFocus = CloseDrawerOnMainFocus,
        FloatingDrawer = FloatingDrawer,
        DrawerPosition = DrawerPosition,
        HeaderPosition = HeaderPosition,
        HeaderColor = HeaderColor,
        DrawerColor = DrawerColor,
        PageFont = PageFont
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
               && _headerColor == other._headerColor 
               && _drawerColor == other._drawerColor 
               && _closeDrawerOnMainFocus == other._closeDrawerOnMainFocus 
               && _closeDrawerOnNavigate == other._closeDrawerOnNavigate 
               && _floatingDrawer == other._floatingDrawer 
               && _pageFont.ToString() == other._pageFont.ToString();
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return obj is LayoutSettings other && Equals(other);
    }

    public override int GetHashCode() => Hash.GetHashCode();

    public static bool operator ==(LayoutSettings? left, LayoutSettings? right) => Equals(left, right);

    public static bool operator !=(LayoutSettings? left, LayoutSettings? right) => !Equals(left, right);
}