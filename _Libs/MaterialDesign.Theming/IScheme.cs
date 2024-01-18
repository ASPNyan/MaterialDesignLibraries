using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Theming;

public interface IScheme
{
    /// <summary>
    /// Null when unavailable
    /// </summary>
    public HCTA? Origin { get; }
    public bool IsDarkScheme { get; }
    public void SetDark();
    public void SetLight();

    public event Action OnUpdate;
    
    public HCTA Primary { get; }
    public HCTA OnPrimary { get; }
    public HCTA PrimaryContainer { get; }
    public HCTA OnPrimaryContainer { get; }
    
    public HCTA Secondary { get; }
    public HCTA OnSecondary { get; }
    public HCTA SecondaryContainer { get; }
    public HCTA OnSecondaryContainer { get; }
    
    public HCTA Tertiary { get; }
    public HCTA OnTertiary { get; }
    public HCTA TertiaryContainer { get; }
    public HCTA OnTertiaryContainer { get; }

    public HCTA Outline { get; }
    public HCTA OutlineVariant { get; }
    
    public HCTA Background { get; }
    public HCTA OnBackground { get; }
    public HCTA Surface { get; }
    public HCTA OnSurface { get; }
    public HCTA SurfaceVariant { get; }
    public HCTA OnSurfaceVariant { get; }
    
    public HCTA SurfaceInverse { get; }
    public HCTA OnSurfaceInverse { get; }
    public HCTA SurfaceBright { get; }
    public HCTA SurfaceDim { get; }
    
    public HCTA SurfaceContainer { get; }
    public HCTA SurfaceContainerLow { get; }
    public HCTA SurfaceContainerLowest { get; }
    public HCTA SurfaceContainerHigh { get; }
    public HCTA SurfaceContainerHighest { get; }

    public HCTA PrimaryFixed { get; }
    public HCTA PrimaryFixedDim { get; }
    public HCTA OnPrimaryFixed { get; }
    public HCTA OnPrimaryFixedVariant { get; }
    
    public HCTA SecondaryFixed { get; }
    public HCTA SecondaryFixedDim { get; }
    public HCTA OnSecondaryFixed { get; }
    public HCTA OnSecondaryFixedVariant { get; }
    
    public HCTA TertiaryFixed { get; }
    public HCTA TertiaryFixedDim { get; }
    public HCTA OnTertiaryFixed { get; }
    public HCTA OnTertiaryFixedVariant { get; }
    
    public HCTA Error => CorePalette.ErrorPalette.GetWithTone(IsDarkScheme ? 80 : 40);
    public HCTA OnError => CorePalette.ErrorPalette.GetWithTone(Error.T + (IsDarkScheme ? -60 : 60));
    public HCTA ErrorContainer => CorePalette.ErrorPalette.GetWithTone(Error.T + (IsDarkScheme ? -50 : 50));
    public HCTA OnErrorContainer => CorePalette.ErrorPalette.GetWithTone(IsDarkScheme ? 90 : 10);
}