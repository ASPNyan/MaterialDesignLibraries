using MaterialDesign.Color.Extensions;
// ReSharper disable UnusedAutoPropertyAccessor.Global | Remove IDE highlighting for unused get accessors

#pragma warning disable CA2208 // Disable in file for all uses. Prevents ArgumentOutOfRange warnings from appearing

namespace MaterialDesign.Color.Schemes.Custom;

public abstract record CustomSchemeBase
{
    public void SetDark()
    {
        IsDark = true;
        OnUpdate?.Invoke();
    }
    
    public void SetLight()
    {
        IsDark = false;
        OnUpdate?.Invoke();
    }

    public void Update(HCTA newColor)
    {
        Construct(newColor);
        OnUpdate?.Invoke();
    }

    public event Action? OnUpdate;
    
    public bool IsDark { get; private set; }

    #nullable disable
    #region Colors

    private bool BlackAndWhiteText { get; set; }

    public HCTA GetText(Func<HCTA> colorMethod)
    {
        HCTA color = colorMethod();
        
        if (BlackAndWhiteText)
        {
            if (color.T <= 50) return new HCTA(0, 0, 0);
            return new HCTA(0, 0, 100);
        }

        return color;
    }
    
    public Func<HCTA> Primary { get; private set; }
    public Func<HCTA> OnPrimary { get; private set; }
    public Func<HCTA> PrimaryContainer { get; private set; }
    public Func<HCTA> OnPrimaryContainer { get; private set; }
    
    public Func<HCTA> Secondary { get; private set; }
    public Func<HCTA> OnSecondary { get; private set; }
    public Func<HCTA> SecondaryContainer { get; private set; }
    public Func<HCTA> OnSecondaryContainer { get; private set; }
    
    public Func<HCTA> Tertiary { get; private set; }
    public Func<HCTA> OnTertiary { get; private set; }
    public Func<HCTA> TertiaryContainer { get; private set; }
    public Func<HCTA> OnTertiaryContainer { get; private set; }

    public Func<HCTA> Outline { get; private set; }
    public Func<HCTA> OutlineVariant { get; private set; }
    
    public Func<HCTA> Background { get; private set; }
    public Func<HCTA> OnBackground { get; private set; }
    public Func<HCTA> SurfaceVariant { get; private set; }
    public Func<HCTA> OnSurfaceVariant { get; private set; }
    
    public Func<HCTA> SurfaceInverse { get; private set; }
    public Func<HCTA> OnSurfaceInverse { get; private set; }
    public Func<HCTA> SurfaceBright { get; private set; }
    public Func<HCTA> SurfaceDim { get; private set; }
    
    public Func<HCTA> SurfaceContainer { get; private set; }
    public Func<HCTA> SurfaceContainerLow { get; private set; }
    public Func<HCTA> SurfaceContainerLowest { get; private set; }
    public Func<HCTA> SurfaceContainerHigh { get; private set; }
    public Func<HCTA> SurfaceContainerHighest { get; private set; }

    private const int FixedTone = 90;
    private const int FixedDimTone = 80;
    private const int OnFixedTone = 10;
    private const int OnFixedBrightTone = 30;

    private static HCTA FixedColor(HCTA color, int tone) => new(color.H, color.C, tone);
    
    public HCTA PrimaryFixed() => FixedColor(Primary(), FixedTone);
    public HCTA PrimaryFixedDim() => FixedColor(Primary(), FixedDimTone);
    public HCTA OnPrimaryFixed() => FixedColor(Primary(), OnFixedTone);
    public HCTA OnPrimaryFixedBright() => FixedColor(Primary(), OnFixedBrightTone);
    
    public HCTA SecondaryFixed() => FixedColor(Secondary(), FixedTone);
    public HCTA SecondaryFixedDim() => FixedColor(Secondary(), FixedDimTone);
    public HCTA OnSecondaryFixed() => FixedColor(Secondary(), OnFixedTone);
    public HCTA OnSecondaryFixedBright() => FixedColor(Secondary(), OnFixedBrightTone);
    
    public HCTA TertiaryFixed() => FixedColor(Tertiary(), FixedTone);
    public HCTA TertiaryFixedDim() => FixedColor(Tertiary(), FixedDimTone);
    public HCTA OnTertiaryFixed() => FixedColor(Tertiary(), OnFixedTone);
    public HCTA OnTertiaryFixedBright() => FixedColor(Tertiary(), OnFixedBrightTone);

    #endregion
    #nullable restore
    
    /// <summary>
    /// Whether the text should be colored or black & white.
    /// </summary>
    protected abstract TextStyleType TextStyle { get; }
    /// <summary>
    /// How the chroma of the source color will be modified. <see cref="SaturationType.MediumSaturation"/> means
    /// the saturation is equal to the source.
    /// </summary>
    protected abstract SaturationType Saturation { get; }
    /// <summary>
    /// The visual distance between dark and light themes. All options still insure at least a minimal contrast of
    /// 4.5 for accessibility.
    /// </summary>
    protected abstract ToneGap DarkLightGap { get; }
    /// <summary>
    /// The visual distance between a color role and its "on" variant (e.g. <see cref="Primary"/> and
    /// <see cref="OnPrimary"/>). All options still insure at least a minimal contrast of 4.5 for
    /// accessibility.
    /// </summary>
    protected abstract ToneGap OnColorGap { get; }
    /// <summary>
    /// The visual distance between a color role its container variant (e.g. <see cref="Primary"/> and
    /// <see cref="PrimaryContainer"/>). All options still insure at least a minimal contrast of 4.5
    /// for accessibility.
    /// </summary>
    protected abstract ToneGap CoreContainerGap { get; }
    
    protected abstract DifferenceFromSource PrimaryDifference { get; }
    protected abstract DifferenceFromSource SecondaryDifference { get; }
    protected abstract DifferenceFromSource TertiaryDifference { get; }
    protected abstract DifferenceFromSource SurfaceDifference { get; }
    /// <summary>
    /// <see cref="SaturationType.MediumSaturation"/> means an equal chroma value. Default is
    /// <see cref="SaturationType.HighSaturation"/>.
    /// </summary>
    protected virtual SaturationType VariantDifferenceFromSurface => SaturationType.HighSaturation;

    protected virtual double PrimaryHue => 0;
    protected virtual double SecondaryHue => 0;
    protected virtual double TertiaryHue => 0;
    protected virtual double SurfaceHue => 0;
    
    protected virtual double PrimaryChroma => 0;
    protected virtual double SecondaryChroma => 0;
    protected virtual double TertiaryChroma => 0;
    protected virtual double SurfaceChroma => 0;

    private double GenerateSourceChroma(HCTA source)
    {
        return Saturation switch
        {
            SaturationType.Desaturated => double.Min(source.C, 6),
            SaturationType.LowSaturation => double.Min(source.C * 0.7, 18),
            SaturationType.MediumSaturation => source.C,
            SaturationType.HighSaturation => double.Max(source.C * 1.5, 54),
            SaturationType.Saturated => double.Max(source.C, source.MaxChroma()),
            _ => throw new ArgumentOutOfRangeException(nameof(Saturation), "Saturation must be one of 5 valid enum values.")
        };
    }

    private HCTA CreateDifferentColor(HCTA source, DifferenceFromSource difference)
    {
        HCTA diff = new(source.H, source.C, source.T);

        if (difference is 0 or DifferenceFromSource.NegativeHueShift) return diff;

        bool negativeHueShift = false;
        foreach (DifferenceFromSource diffType in Enum.GetValues<DifferenceFromSource>())
        {
            const int saturateSmall = 5;
            const int saturate = 8;
            const int saturateLarge = 12;
            
            switch (diffType)
            {
                case DifferenceFromSource.NegativeHueShift:
                    negativeHueShift = true;
                    break;
                case DifferenceFromSource.HueShiftSmall:
                    const int hueShiftSmall = 15;
                    diff.H += negativeHueShift ? -hueShiftSmall : hueShiftSmall;
                    break;
                case DifferenceFromSource.HueShift:
                    const int hueShift = 30;
                    diff.H += negativeHueShift ? -hueShift : hueShift;
                    break;
                case DifferenceFromSource.HueShiftWide:
                    const int hueShiftWide = 50;
                    diff.H += negativeHueShift ? -hueShiftWide : hueShiftWide;
                    break;
                case DifferenceFromSource.UsePrimaryHueOverride:
                    diff.H = PrimaryHue;
                    break;
                case DifferenceFromSource.UseSecondaryHueOverride:
                    diff.H = SecondaryHue;
                    break;
                case DifferenceFromSource.UseTertiaryHueOverride:
                    diff.H = TertiaryHue;
                    break;
                case DifferenceFromSource.UseSurfaceHueOverride:
                    diff.H = SurfaceHue;
                    break; 
                case DifferenceFromSource.RelativeDesaturateSmall:
                    diff.C -= saturateSmall;
                    break;
                case DifferenceFromSource.RelativeDesaturate:
                    diff.C -= saturate;
                    break;
                case DifferenceFromSource.RelativeDesaturateLarge:
                    diff.C -= saturateLarge;
                    break;
                case DifferenceFromSource.RelativeSaturateSmall:
                    diff.C += saturateSmall;
                    break;
                case DifferenceFromSource.RelativeSaturate:
                    diff.C += saturate;
                    break;
                case DifferenceFromSource.RelativeSaturateLarge:
                    diff.C += saturateLarge;
                    break;
                case DifferenceFromSource.UsePrimaryChromaOverride:
                    diff.C = PrimaryChroma;
                    break;
                case DifferenceFromSource.UseSecondaryChromaOverride:
                    diff.C = SecondaryChroma;
                    break;
                case DifferenceFromSource.UseTertiaryChromaOverride:
                    diff.C = TertiaryChroma;
                    break;
                case DifferenceFromSource.UseSurfaceChromaOverride:
                    diff.C = SurfaceChroma;
                    break;
            }
        }

        return diff;
    }

    protected CustomSchemeBase(HCTA source) => Construct(source);
    
    private void Construct(HCTA source)
    {
        HCTA modSource = new(source.H, GenerateSourceChroma(source), source.T);

        HCTA primarySource = CreateDifferentColor(modSource, PrimaryDifference);
        HCTA secondarySource = CreateDifferentColor(modSource, SecondaryDifference);
        HCTA tertiarySource = CreateDifferentColor(modSource, TertiaryDifference);
        HCTA surfaceSource = CreateDifferentColor(modSource, SurfaceDifference);

        double onColorContrastLevel = ToneGapValues(OnColorGap, nameof(OnColorGap));
        double coreContainerContrastLevel = ToneGapValues(CoreContainerGap, nameof(CoreContainerGap));
        double darkTone = (100 - new HCTA(0, 0, 0).ContrastTo(ToneGapValues(DarkLightGap, nameof(DarkLightGap))).T) / 2;
        double lightTone = 100 - darkTone;

        CustomSourcePalette primaryPalette = CoreContainerGenerator(primarySource);
        CustomSourcePalette secondaryPalette = CoreContainerGenerator(secondarySource);
        CustomSourcePalette tertiaryPalette = CoreContainerGenerator(tertiarySource);
        TonalPalette surfacePalette = new(surfaceSource);
        TonalPalette surfaceVariantPalette = new(new HCTA(surfacePalette.Hue, GetSurfaceVariantChroma(), 50));

        BlackAndWhiteText = TextStyle is TextStyleType.BlackAndWhite;
        Primary = CustomSourcePaletteMethod(primaryPalette.Core);
        OnPrimary = CustomSourcePaletteMethod(primaryPalette.OnCore);
        PrimaryContainer = CustomSourcePaletteMethod(primaryPalette.Container);
        OnPrimaryContainer = CustomSourcePaletteMethod(primaryPalette.OnContainer);
        Secondary = CustomSourcePaletteMethod(secondaryPalette.Core);
        OnSecondary = CustomSourcePaletteMethod(secondaryPalette.OnCore);
        SecondaryContainer = CustomSourcePaletteMethod(secondaryPalette.Container);
        OnSecondaryContainer = CustomSourcePaletteMethod(secondaryPalette.OnContainer);
        Tertiary = CustomSourcePaletteMethod(tertiaryPalette.Core);
        OnTertiary = CustomSourcePaletteMethod(tertiaryPalette.OnCore);
        TertiaryContainer = CustomSourcePaletteMethod(tertiaryPalette.Container);
        OnTertiaryContainer = CustomSourcePaletteMethod(tertiaryPalette.OnContainer);
        Outline = () => surfaceVariantPalette.GetWithTone(IsDark ? 60 : 50);
        OutlineVariant = () => surfaceVariantPalette.GetWithTone(IsDark ? 80 : 30);
        Background = () => surfacePalette.GetWithTone(IsDark ? 6 : 98);
        OnBackground = () => surfacePalette.GetWithTone(IsDark ? 90 : 10);
        SurfaceVariant = () => surfaceVariantPalette.GetWithTone(IsDark ? 30 : 90);
        OnSurfaceVariant = () => surfaceVariantPalette.GetWithTone(IsDark ? 80 : 30);
        SurfaceInverse = () => surfacePalette.GetWithTone(IsDark ? 90 : 20);
        OnSurfaceInverse = () => surfacePalette.GetWithTone(IsDark ? 90 : 20);
        SurfaceBright = () => surfacePalette.GetWithTone(IsDark ? 90 : 20);
        SurfaceDim = () => surfacePalette.GetWithTone(IsDark ? 90 : 20);
        SurfaceContainer = () => surfacePalette.GetWithTone(IsDark ? 12 : 94);
        SurfaceContainerLow = () => surfacePalette.GetWithTone(IsDark ? 10 : 96);
        SurfaceContainerLowest = () => surfacePalette.GetWithTone(IsDark ? 4 : 100);
        SurfaceContainerHigh = () => surfacePalette.GetWithTone(IsDark ? 17 : 92);
        SurfaceContainerHighest = () => surfacePalette.GetWithTone(IsDark ? 22 : 90);

        return;

        double ToneGapValues(ToneGap toneGap, string paramName)
        {
            return toneGap switch
            {
                ToneGap.Minimal => 4.5,
                ToneGap.Narrow => 6.6,
                ToneGap.Broad => 9.0,
                ToneGap.Wide => 12.3,
                _ => throw new ArgumentOutOfRangeException(paramName, 
                    $"{paramName} must be one of four valid values.")
            };
        }
        
        CustomSourcePalette CoreContainerGenerator(HCTA sourceColor) => new(new TonalPalette(sourceColor),
            darkTone, lightTone, onColorContrastLevel, coreContainerContrastLevel);

        Func<HCTA> CustomSourcePaletteMethod(Func<bool, HCTA> method) => () => method(IsDark);

        double GetSurfaceVariantChroma()
        {
            return (VariantDifferenceFromSurface switch
            {
                SaturationType.Desaturated => CreateDifferentColor(surfacePalette.KeyColor, DifferenceFromSource.RelativeDesaturate),
                SaturationType.LowSaturation => CreateDifferentColor(surfacePalette.KeyColor, DifferenceFromSource.RelativeDesaturateSmall),
                SaturationType.MediumSaturation => surfacePalette.KeyColor,
                SaturationType.HighSaturation => CreateDifferentColor(surfacePalette.KeyColor, DifferenceFromSource.RelativeSaturate),
                SaturationType.Saturated => CreateDifferentColor(surfacePalette.KeyColor, DifferenceFromSource.RelativeSaturateLarge),
                _ => throw new ArgumentOutOfRangeException(nameof(VariantDifferenceFromSurface),
                    "VariantDifferenceFromSurface must be one of 5 valid enum values.")
            }).C;
        }
    }

    protected enum TextStyleType
    {
        Colored,
        BlackAndWhite
    }

    protected enum SaturationType
    {
        Desaturated,
        LowSaturation,
        MediumSaturation,
        HighSaturation,
        Saturated
    }

    protected enum ToneGap
    {
        Minimal, // 4.5 contrast
        Narrow, // 6.6 contrast
        Broad, // 9.0 contrast
        Wide // 12.3 contrast
    }

    [Flags]
    protected enum DifferenceFromSource
    {
        NegativeHueShift = 1<<0,
        
        HueShiftSmall = 1<<1, // 15deg
        HueShift = 1<<2, // 30deg
        HueShiftWide = 1<<3, // 50deg
        
        UsePrimaryHueOverride = 1<<4,
        UseSecondaryHueOverride = 1<<5,
        UseTertiaryHueOverride = 1<<6,
        UseSurfaceHueOverride = 1<<7,

        RelativeDesaturateSmall = 1<<8, // -5 chroma
        RelativeDesaturate = 1<<9, // -8 chroma
        RelativeDesaturateLarge = 1<<10, // -12 chroma

        RelativeSaturateSmall = 1<<11, // +5 chroma
        RelativeSaturate = 1<<12, // +8 chroma
        RelativeSaturateLarge = 1<<13, // +12 chroma

        UsePrimaryChromaOverride = 1<<14,
        UseSecondaryChromaOverride = 1<<15,
        UseTertiaryChromaOverride = 1<<16,
        UseSurfaceChromaOverride = 1<<17
    }
}