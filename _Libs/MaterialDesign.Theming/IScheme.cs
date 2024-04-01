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
    
    /// <summary>
    /// Controls whether the current scheme has black and white text (when <c>true</c>)
    /// or colored text (when <c>false</c>). Colored text is the default. 
    /// </summary>
    public TextColorStyle TextColorStyle => TextColorStyle.Colored;

    /// <summary>
    /// Sets the current theme's text to <see cref="TextColorStyle.Colored"/>. Does nothing if this is already the case.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the method has not been implemented by current scheme.
    /// </exception>
    public void SetColoredText() =>
        throw new InvalidOperationException("SetColoredText() has not been implemented for this IScheme implementation.");

    /// <summary>
    /// Sets the current theme's text to <see cref="TextColorStyle.BlackAndWhite">Black &amp; White</see>.
    /// Does nothing if this is already the case.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the method has not been implemented by current scheme.
    /// </exception>
    public void SetBlackAndWhiteText() =>
        throw new InvalidOperationException("SetBlackAndWhiteText() has not been implemented for this IScheme implementation.");

    /// <summary>
    /// Sets the current theme's text to <see cref="TextColorStyle.GreyAndGrey">Dark &amp; Light Grey</see>.
    /// Does nothing if this is already the case.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the method has not been implemented by current scheme.
    /// </exception>
    public void SetGreyText() =>
        throw new InvalidOperationException("SetBlackAndWhiteText() has not been implemented for this IScheme implementation.");

    /// <summary>
    /// Converts a color into the correct text color as per <see cref="TextColorStyle"/>.
    /// </summary>
    public HCTA GetTextColor(HCTA hcta)
    {
        return TextColorStyle switch
        {
            TextColorStyle.Colored => hcta,
            TextColorStyle.BlackAndWhite => new HCTA(0, 0, hcta.T < 50 ? 0 : 100),
            TextColorStyle.GreyAndGrey => new HCTA(0, 0, hcta.T < 50 ? Math.Min(hcta.T, 21.5) : Math.Max(hcta.T, 82)),
            _ => hcta
        };
    }

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