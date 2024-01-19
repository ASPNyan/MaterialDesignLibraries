using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Schemes.Custom;

namespace MaterialDesignCustomSchemes;

public sealed record CustomScheme(HCTA Source) : CustomSchemeBase(Source)
{
    protected override TextStyleType TextStyle => TextStyleType.BlackAndWhite;
    protected override SaturationType Saturation => SaturationType.Saturated;
    protected override ToneGap DarkLightGap => ToneGap.Minimal;
    protected override ToneGap OnColorGap => ToneGap.Broad;
    protected override ToneGap CoreContainerGap => ToneGap.Narrow;
    protected override DifferenceFromSource PrimaryDifference => DifferenceFromSource.None;
    protected override DifferenceFromSource SecondaryDifference => DifferenceFromSource.RelativeDesaturateLarge;
    protected override DifferenceFromSource TertiaryDifference => DifferenceFromSource.HueShiftWide | DifferenceFromSource.NegativeHueShift;
    protected override DifferenceFromSource SurfaceDifference => DifferenceFromSource.UseSurfaceChromaOverride;
    protected override double SurfaceChroma => double.Min(Source.C, 18);

    public bool Equals(CustomScheme? other)
    {
        return other is not null && Source == other.Source;
    }

    public override int GetHashCode() => Source.GetHashCode();
}