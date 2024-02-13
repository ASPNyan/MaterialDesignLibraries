﻿using static MaterialDesign.Color.Schemes.Custom.CustomSchemeBase.DifferenceFromSource;

namespace MaterialDesign.Color.Schemes.Custom;

public record ModifiableCustomScheme(HCTA Origin) : CustomSchemeBase(Origin)
{
    public new HCTA Origin => base.Origin!;

    private TextStyleType _schemeTextStyle = TextStyleType.BlackAndWhite;
    private SaturationType _schemeSaturation = SaturationType.Saturated;
    private ToneGap _schemeDarkLightGap = ToneGap.Minimal;
    private ToneGap _schemeOnColorGap = ToneGap.Broad;
    private ToneGap _schemeCoreContainerGap = ToneGap.Narrow;
    private DifferenceFromSource _schemePrimaryDifference = None;
    private DifferenceFromSource _schemeSecondaryDifference = RelativeDesaturateLarge;
    private DifferenceFromSource _schemeTertiaryDifference = HueShiftWide | NegativeHueShift;
    private DifferenceFromSource _schemeSurfaceDifference = UseSurfaceChromaOverride;
    private SaturationType _schemeVariantDifferenceFromSurface = SaturationType.HighSaturation;
    private double _schemePrimaryHue;
    private double _schemeSecondaryHue;
    private double _schemeTertiaryHue;
    private double _schemeSurfaceHue;
    private double _schemePrimaryChroma;
    private double _schemeSecondaryChroma;
    private double _schemeTertiaryChroma;
    private double _schemeSurfaceChroma;

    public double Hue
    {
        get => Origin.H;
        set => UpdateSettings(Origin.H = value);
    }
    
    public double Chroma
    {
        get => Origin.C;
        set => UpdateSettings(Origin.C = value);
    }
    
    public double Tone
    {
        get => Origin.T;
        set => UpdateSettings(Origin.T = value);
    }

    public TextStyleType SchemeTextStyle
    {
        get => _schemeTextStyle;
        set => UpdateSettings(out _schemeTextStyle, value);
    }

    public SaturationType SchemeSaturation
    {
        get => _schemeSaturation;
        set => UpdateSettings(out _schemeSaturation, value);
    }

    public ToneGap SchemeDarkLightGap
    {
        get => _schemeDarkLightGap;
        set => UpdateSettings(out _schemeDarkLightGap, value);
    }

    public ToneGap SchemeOnColorGap
    {
        get => _schemeOnColorGap;
        set => UpdateSettings(out _schemeOnColorGap, value);
    }

    public ToneGap SchemeCoreContainerGap
    {
        get => _schemeCoreContainerGap;
        set => UpdateSettings(out _schemeCoreContainerGap, value);
    }

    public DifferenceFromSource SchemePrimaryDifference
    {
        get => _schemePrimaryDifference;
        set => UpdateSettings(out _schemePrimaryDifference, value);
    }

    public DifferenceFromSource SchemeSecondaryDifference
    {
        get => _schemeSecondaryDifference;
        set => UpdateSettings(out _schemeSecondaryDifference, value);
    }

    public DifferenceFromSource SchemeTertiaryDifference
    {
        get => _schemeTertiaryDifference;
        set => UpdateSettings(out _schemeTertiaryDifference, value);
    }

    public DifferenceFromSource SchemeSurfaceDifference
    {
        get => _schemeSurfaceDifference;
        set => UpdateSettings(out _schemeSurfaceDifference, value);
    }

    public SaturationType SchemeVariantDifferenceFromSurface
    {
        get => _schemeVariantDifferenceFromSurface;
        set => UpdateSettings(out _schemeVariantDifferenceFromSurface, value);
    }

    public double SchemePrimaryHue
    {
        get => _schemePrimaryHue;
        set => UpdateSettings(out _schemePrimaryHue, value);
    }

    public double SchemeSecondaryHue
    {
        get => _schemeSecondaryHue;
        set => UpdateSettings(out _schemeSecondaryHue, value);
    }

    public double SchemeTertiaryHue
    {
        get => _schemeTertiaryHue;
        set => UpdateSettings(out _schemeTertiaryHue, value);
    }

    public double SchemeSurfaceHue
    {
        get => _schemeSurfaceHue;
        set => UpdateSettings(out _schemeSurfaceHue, value);
    }

    public double SchemePrimaryChroma
    {
        get => _schemePrimaryChroma;
        set => UpdateSettings(out _schemePrimaryChroma, value);
    }

    public double SchemeSecondaryChroma
    {
        get => _schemeSecondaryChroma;
        set => UpdateSettings(out _schemeSecondaryChroma, value);
    }

    public double SchemeTertiaryChroma
    {
        get => _schemeTertiaryChroma;
        set => UpdateSettings(out _schemeTertiaryChroma, value);
    }

    public double SchemeSurfaceChroma
    {
        get => _schemeSurfaceChroma;
        set => UpdateSettings(out _schemeSurfaceChroma, value);
    }

    protected override TextStyleType TextStyle => SchemeTextStyle;
    protected override SaturationType Saturation => SchemeSaturation;
    protected override ToneGap DarkLightGap => SchemeDarkLightGap;
    protected override ToneGap OnColorGap => SchemeOnColorGap;
    protected override ToneGap CoreContainerGap => SchemeCoreContainerGap;
    protected override DifferenceFromSource PrimaryDifference => SchemePrimaryDifference;
    protected override DifferenceFromSource SecondaryDifference => SchemeSecondaryDifference;
    protected override DifferenceFromSource TertiaryDifference => SchemeTertiaryDifference;
    protected override DifferenceFromSource SurfaceDifference => SchemeSurfaceDifference;
    protected override SaturationType VariantDifferenceFromSurface => SchemeVariantDifferenceFromSurface;

    protected override double PrimaryHue => SchemePrimaryHue;
    protected override double SecondaryHue => SchemeSecondaryHue;
    protected override double TertiaryHue => SchemeTertiaryHue;
    protected override double SurfaceHue => SchemeSurfaceHue;
    protected override double PrimaryChroma => SchemePrimaryChroma;
    protected override double SecondaryChroma => SchemeSecondaryChroma;
    protected override double TertiaryChroma => SchemeTertiaryChroma;
    protected override double SurfaceChroma => SchemeSurfaceChroma;

    private void UpdateSettings<T>(out T var, T val)
    {
        var = val;
        Update(Origin);
    }
    
    private void UpdateSettings<T>(T _) => Update(Origin);

    public override int GetHashCode() => Origin.GetHashCode();
}