using MaterialDesign.Color.Extensions;
using MaterialDesign.Theming;
using MaterialDesign.Theming.Injection;

#pragma warning disable CA2208 // Disable in file for all uses. Prevents ArgumentOutOfRange warnings from appearing

namespace MaterialDesign.Color.Schemes.Custom;

public abstract record CustomSchemeBase : IThemeSource, IScheme
{
    public HCTA? Origin { get; private set; }
    
    public void SetDark()
    {
        IsDarkScheme = true;
        Construct(Origin!);
        OnUpdate?.Invoke();
    }
    
    public void SetLight()
    {
        IsDarkScheme = false;
        Construct(Origin!);
        OnUpdate?.Invoke();
    }

    public void Update(HCTA newColor)
    {
        Construct(newColor);
        OnUpdate?.Invoke();
    }

    public event Action? OnUpdate;
    
    public bool IsDarkScheme { get; private set; }

    #nullable disable
    #region Colors

    private bool BlackAndWhiteText { get; set; }

    public HCTA GetText(HCTA color)
    {
        if (!BlackAndWhiteText) return color;
        return color.T <= 50 ? new HCTA(0, 0, 0) : new HCTA(0, 0, 100);
    }
    
    public HCTA Primary { get; private set; }
    public HCTA OnPrimary { get; private set; }
    public HCTA PrimaryContainer { get; private set; }
    public HCTA OnPrimaryContainer { get; private set; }
    
    public HCTA Secondary { get; private set; }
    public HCTA OnSecondary { get; private set; }
    public HCTA SecondaryContainer { get; private set; }
    public HCTA OnSecondaryContainer { get; private set; }
    
    public HCTA Tertiary { get; private set; }
    public HCTA OnTertiary { get; private set; }
    public HCTA TertiaryContainer { get; private set; }
    public HCTA OnTertiaryContainer { get; private set; }

    public HCTA Outline { get; private set; }
    public HCTA OutlineVariant { get; private set; }
    
    public HCTA Background { get; private set; }
    public HCTA OnBackground { get; private set; }
    public HCTA Surface => Background;
    public HCTA OnSurface => OnBackground;
    public HCTA SurfaceVariant { get; private set; }
    public HCTA OnSurfaceVariant { get; private set; }
    
    public HCTA SurfaceInverse { get; private set; }
    public HCTA OnSurfaceInverse { get; private set; }
    public HCTA SurfaceBright { get; private set; }
    public HCTA SurfaceDim { get; private set; }
    
    public HCTA SurfaceContainer { get; private set; }
    public HCTA SurfaceContainerLow { get; private set; }
    public HCTA SurfaceContainerLowest { get; private set; }
    public HCTA SurfaceContainerHigh { get; private set; }
    public HCTA SurfaceContainerHighest { get; private set; }

    private const int FixedTone = 90;
    private const int FixedDimTone = 80;
    private const int OnFixedTone = 10;
    private const int OnFixedBrightTone = 30;

    private static HCTA FixedColor(HCTA color, int tone) => new(color.H, color.C, tone);
    
    public HCTA PrimaryFixed => FixedColor(Primary, FixedTone);
    public HCTA PrimaryFixedDim => FixedColor(Primary, FixedDimTone);
    public HCTA OnPrimaryFixed => FixedColor(Primary, OnFixedTone);
    public HCTA OnPrimaryFixedVariant => FixedColor(Primary, OnFixedBrightTone);
    
    public HCTA SecondaryFixed => FixedColor(Secondary, FixedTone);
    public HCTA SecondaryFixedDim => FixedColor(Secondary, FixedDimTone);
    public HCTA OnSecondaryFixed => FixedColor(Secondary, OnFixedTone);
    public HCTA OnSecondaryFixedVariant => FixedColor(Secondary, OnFixedBrightTone);
    
    public HCTA TertiaryFixed => FixedColor(Tertiary, FixedTone);
    public HCTA TertiaryFixedDim => FixedColor(Tertiary, FixedDimTone);
    public HCTA OnTertiaryFixed => FixedColor(Tertiary, OnFixedTone);
    public HCTA OnTertiaryFixedVariant => FixedColor(Tertiary, OnFixedBrightTone);

    #endregion
    #nullable restore
    
    /// <summary>
    /// Whether the text should be colored or black &amp; white.
    /// </summary>
    protected internal abstract TextStyleType TextStyle { get; }
    /// <summary>
    /// How the chroma of the source color will be modified. <see cref="SaturationType.MediumSaturation"/> means
    /// the saturation is equal to the source.
    /// </summary>
    protected internal abstract SaturationType Saturation { get; }
    /// <summary>
    /// The visual distance between dark and light themes. All options still insure at least a minimal contrast of
    /// 4.5 for accessibility.
    /// </summary>
    protected internal abstract ToneGap DarkLightGap { get; }
    /// <summary>
    /// The visual distance between a color role and its "on" variant (e.g. <see cref="Primary"/> and
    /// <see cref="OnPrimary"/>). All options still insure at least a minimal contrast of 4.5 for
    /// accessibility.
    /// </summary>
    protected internal abstract ToneGap OnColorGap { get; }
    /// <summary>
    /// The visual distance between a color role its container variant (e.g. <see cref="Primary"/> and
    /// <see cref="PrimaryContainer"/>). All options still insure at least a minimal contrast of 4.5
    /// for accessibility.
    /// </summary>
    protected internal abstract ToneGap CoreContainerGap { get; }
    
    protected internal abstract DifferenceFromSource PrimaryDifference { get; }
    protected internal abstract DifferenceFromSource SecondaryDifference { get; }
    protected internal abstract DifferenceFromSource TertiaryDifference { get; }
    protected internal abstract DifferenceFromSource SurfaceDifference { get; }
    /// <summary>
    /// <see cref="SaturationType.MediumSaturation"/> means an equal chroma value. Default is
    /// <see cref="SaturationType.HighSaturation"/>.
    /// </summary>
    protected internal virtual SaturationType VariantDifferenceFromSurface => SaturationType.HighSaturation;

    protected internal virtual double PrimaryHue { get; set; } = 0;
    protected internal virtual double SecondaryHue { get; set; } = 0;
    protected internal virtual double TertiaryHue { get; set; } = 0;
    protected internal virtual double SurfaceHue { get; set; } = 0;
    
    protected internal virtual double PrimaryChroma { get; set; } = 0;
    protected internal virtual double SecondaryChroma { get; set; } = 0;
    protected internal virtual double TertiaryChroma { get; set; } = 0;
    protected internal virtual double SurfaceChroma { get; set; } = 0;

    protected virtual void PreConstruct()
    {
    }

    protected virtual void PostConstruct()
    {
    }

    private double GenerateSourceChroma(HCTA source)
    {
        return Saturation switch
        {
            SaturationType.Desaturated => double.Min(source.C, source.MaxChroma() * 0.1),
            SaturationType.LowSaturation => double.Min(source.C, source.MaxChroma() * 0.25),
            SaturationType.MediumSaturation => source.C,
            SaturationType.HighSaturation => double.Max(source.C, source.MaxChroma() * 0.7),
            SaturationType.Saturated => source.MaxChroma(),
            _ => throw new ArgumentOutOfRangeException(nameof(Saturation), "Saturation must be one of 5 valid enum values.")
        };
    }

    private HCTA CreateDifferentColor(HCTA source, DifferenceFromSource difference)
    {
        HCTA diff = new(source.H, double.Min(source.C, source.MaxChroma()), source.T);

        if (difference is 0 or DifferenceFromSource.NegativeHueShift or DifferenceFromSource.None) return diff;

        bool negativeHueShift = false;
        foreach (DifferenceFromSource diffType in Enum.GetValues<DifferenceFromSource>())
        {
            double cRatio = diff.GetChromaDistanceRatio();
            double saturateSmall = cRatio * 0.25;
            double saturate = cRatio * 0.55;
            double saturateLarge = cRatio * 0.85;

            double bonus = diff.C > diff.ExactMaxChroma(3) * 0.4 ? 1 : (100 - cRatio) / 100 + 1;

            if ((difference & diffType) == 0) continue;
            
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
                    diff.C = GetChromaAfterRatioChange(-saturateSmall);
                    break;
                case DifferenceFromSource.RelativeDesaturate:
                    diff.C = GetChromaAfterRatioChange(-saturate);
                    break;
                case DifferenceFromSource.RelativeDesaturateLarge:
                    diff.C = GetChromaAfterRatioChange(-saturateLarge);
                    break;
                case DifferenceFromSource.RelativeSaturateSmall:
                    diff.C = GetChromaAfterRatioChange(saturateSmall * Math.Pow(bonus, 1.3));
                    break;
                case DifferenceFromSource.RelativeSaturate:
                    diff.C = GetChromaAfterRatioChange(saturate * Math.Pow(bonus, 1.4));
                    break;
                case DifferenceFromSource.RelativeSaturateLarge:
                    diff.C = GetChromaAfterRatioChange(saturateLarge * Math.Pow(bonus, 1.6));
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
        
        double GetChromaAfterRatioChange(double ratioChange)
        {
            double chromaRatio = diff.GetChromaDistanceRatio();
            
            return diff.CreateFromChromaDistanceRatio(chromaRatio + ratioChange).C;
        }
    }

    private void ConstructSourcePalettes(HCTA modSource, out CustomSourcePalette primaryPalette, 
        out CustomSourcePalette secondaryPalette, out CustomSourcePalette tertiaryPalette)
    {
        double onColorContrastLevel = ToneGapValues(OnColorGap, nameof(OnColorGap));
        double coreContainerContrastLevel = ToneGapValues(CoreContainerGap, nameof(CoreContainerGap));
        double darkLightContrastLevel = ToneGapValues(DarkLightGap, nameof(DarkLightGap));
        
        if (darkLightContrastLevel < onColorContrastLevel) darkLightContrastLevel = onColorContrastLevel;
        
        double darkTone = (100 - new HCTA(0, 0, 0).ContrastTo(darkLightContrastLevel).T) / 2;
        double lightTone = 100 - darkTone;

        double containerContrastLevel = Contrast.Contrast.RatioOfTones(
            new HCTA(0, 0, darkTone).ContrastTo(coreContainerContrastLevel, false).T, 0);
        if (containerContrastLevel < onColorContrastLevel) coreContainerContrastLevel = onColorContrastLevel;
        
        primaryPalette = CoreContainerGenerator(PrimaryDifference);
        secondaryPalette = CoreContainerGenerator(SecondaryDifference);
        tertiaryPalette = CoreContainerGenerator(TertiaryDifference);

        return;
        
        static double ToneGapValues(ToneGap toneGap, string paramName)
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
        
        CustomSourcePalette CoreContainerGenerator(DifferenceFromSource diff) => new(new TonalPalette(modSource),
            darkTone, lightTone, onColorContrastLevel, coreContainerContrastLevel, hcta => CreateDifferentColor(hcta, diff));
    }

    // ReSharper disable once NotNullOrRequiredMemberIsNotInitialized
    protected CustomSchemeBase(HCTA source)
    {
        Origin = source;
        Construct(source);
    }

    private void Construct(HCTA source)
    {
        Origin = source;
        PreConstruct();
        HCTA modSource = new(source.H, GenerateSourceChroma(source), source.T);
        
        ConstructSourcePalettes(modSource, out CustomSourcePalette primaryPalette, 
            out CustomSourcePalette secondaryPalette, out CustomSourcePalette tertiaryPalette);
        
        HCTA surfaceSource = CreateDifferentColor(modSource, SurfaceDifference);
        TonalPalette surfacePalette = new(surfaceSource);
        TonalPalette surfaceVariantPalette = new(surfacePalette.Hue, GetSurfaceVariantChroma());

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
        Outline = surfaceVariantPalette.GetWithTone(IsDarkScheme ? 60 : 50);
        OutlineVariant = surfaceVariantPalette.GetWithTone(IsDarkScheme ? 80 : 30);
        Background = surfacePalette.GetWithTone(IsDarkScheme ? 6 : 98);
        OnBackground = surfacePalette.GetWithTone(IsDarkScheme ? 90 : 10);
        SurfaceVariant = surfaceVariantPalette.GetWithTone(IsDarkScheme ? 30 : 90);
        OnSurfaceVariant = surfaceVariantPalette.GetWithTone(IsDarkScheme ? 80 : 30);
        SurfaceInverse = surfacePalette.GetWithTone(IsDarkScheme ? 90 : 20);
        OnSurfaceInverse = surfacePalette.GetWithTone(IsDarkScheme ? 90 : 20);
        SurfaceBright = surfacePalette.GetWithTone(IsDarkScheme ? 90 : 20);
        SurfaceDim = surfacePalette.GetWithTone(IsDarkScheme ? 90 : 20);
        SurfaceContainer = surfacePalette.GetWithTone(IsDarkScheme ? 12 : 94);
        SurfaceContainerLow = surfacePalette.GetWithTone(IsDarkScheme ? 10 : 96);
        SurfaceContainerLowest = surfacePalette.GetWithTone(IsDarkScheme ? 4 : 100);
        SurfaceContainerHigh = surfacePalette.GetWithTone(IsDarkScheme ? 17 : 92);
        SurfaceContainerHighest = surfacePalette.GetWithTone(IsDarkScheme ? 22 : 90);

        PostConstruct();
        
        return;

        HCTA CustomSourcePaletteMethod(Func<bool, HCTA> method) => method(IsDarkScheme);

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

    public enum TextStyleType
    {
        Colored,
        BlackAndWhite
    }

    public enum SaturationType
    {
        Desaturated,
        LowSaturation,
        MediumSaturation,
        HighSaturation,
        Saturated
    }

    public enum ToneGap
    {
        Minimal, // 4.5 contrast
        Narrow, // 6.6 contrast
        Broad, // 9.0 contrast
        Wide // 12.3 contrast
    }

    [Flags]
    public enum DifferenceFromSource
    {
        None = 0,
        
        NegativeHueShift = 1<<0,
        
        /// <summary>
        /// 15deg
        /// </summary>
        HueShiftSmall = 1<<1, 
        /// <summary>
        /// 30deg
        /// </summary>
        HueShift = 1<<2, 
        /// <summary>
        /// 50deg
        /// </summary>
        HueShiftWide = 1<<3,
        
        UsePrimaryHueOverride = 1<<4,
        UseSecondaryHueOverride = 1<<5,
        UseTertiaryHueOverride = 1<<6,
        UseSurfaceHueOverride = 1<<7,

        RelativeDesaturateSmall = 1<<8,
        RelativeDesaturate = 1<<9,
        RelativeDesaturateLarge = 1<<10,

        RelativeSaturateSmall = 1<<11,
        RelativeSaturate = 1<<12,
        RelativeSaturateLarge = 1<<13,

        UsePrimaryChromaOverride = 1<<14,
        UseSecondaryChromaOverride = 1<<15,
        UseTertiaryChromaOverride = 1<<16,
        UseSurfaceChromaOverride = 1<<17
    }
}