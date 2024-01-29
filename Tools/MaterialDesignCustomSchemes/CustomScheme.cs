using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Schemes.Custom;

namespace MaterialDesignCustomSchemes;

public sealed record CustomScheme(HCTA Source) : CustomSchemeBase(Source)
{
    public HCTA Source => Origin!;

    public TextStyleType SchemeTextStyle = TextStyleType.BlackAndWhite;
    public SaturationType SchemeSaturation = SaturationType.Saturated;
    public ToneGap SchemeDarkLightGap = ToneGap.Minimal;
    public ToneGap SchemeOnColorGap = ToneGap.Broad;
    public ToneGap SchemeCoreContainerGap = ToneGap.Narrow;
    public DifferenceFromSource SchemePrimaryDifference = DifferenceFromSource.None;
    public DifferenceFromSource SchemeSecondaryDifference = DifferenceFromSource.RelativeDesaturateLarge;
    public DifferenceFromSource SchemeTertiaryDifference = DifferenceFromSource.HueShiftWide | DifferenceFromSource.NegativeHueShift;
    public DifferenceFromSource SchemeSurfaceDifference = DifferenceFromSource.UseSurfaceChromaOverride;
    
    public double SchemePrimaryHue;
    public double SchemeSecondaryHue;
    public double SchemeTertiaryHue;
    public double SchemeSurfaceHue;
    public double SchemePrimaryChroma;
    public double SchemeSecondaryChroma;
    public double SchemeTertiaryChroma;
    public double SchemeSurfaceChroma = 16;

    protected override TextStyleType TextStyle => SchemeTextStyle;
    protected override SaturationType Saturation => SchemeSaturation;
    protected override ToneGap DarkLightGap => SchemeDarkLightGap;
    protected override ToneGap OnColorGap => SchemeOnColorGap;
    protected override ToneGap CoreContainerGap => SchemeCoreContainerGap;
    protected override DifferenceFromSource PrimaryDifference => SchemePrimaryDifference;
    protected override DifferenceFromSource SecondaryDifference => SchemeSecondaryDifference;
    protected override DifferenceFromSource TertiaryDifference => SchemeTertiaryDifference;
    protected override DifferenceFromSource SurfaceDifference => SchemeSurfaceDifference;

    protected override double PrimaryHue => SchemePrimaryHue;
    protected override double SecondaryHue => SchemeSecondaryHue;
    protected override double TertiaryHue => SchemeTertiaryHue;
    protected override double SurfaceHue => SchemeSurfaceHue;
    protected override double PrimaryChroma => SchemePrimaryChroma;
    protected override double SecondaryChroma => SchemeSecondaryChroma;
    protected override double TertiaryChroma => SchemeTertiaryChroma;
    protected override double SurfaceChroma => SchemeSurfaceChroma;

    public void AfterSettingsUpdate() => Update(Source);

    public bool Equals(CustomScheme? other) => other is not null && Source == other.Source;

    public override int GetHashCode() => Source.GetHashCode();
}