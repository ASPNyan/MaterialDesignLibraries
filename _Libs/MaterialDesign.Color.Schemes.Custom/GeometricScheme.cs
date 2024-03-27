namespace MaterialDesign.Color.Schemes.Custom;

public sealed record GeometricScheme : CustomSchemeBase
{
    public ColorGeometry ColorGeometry { get; }

    protected internal override TextStyleType TextStyle { get; }
    protected internal override SaturationType Saturation { get; }
    protected internal override ToneGap DarkLightGap { get; }
    protected internal override ToneGap OnColorGap { get; }
    protected internal override ToneGap CoreContainerGap { get; }
    protected internal override DifferenceFromSource PrimaryDifference => DifferenceFromSource.None;
    protected internal override DifferenceFromSource SecondaryDifference { get; }
    protected internal override DifferenceFromSource TertiaryDifference { get; }
    protected internal override DifferenceFromSource SurfaceDifference { get; }

    protected internal override double SecondaryHue { get; set; }
    protected internal override double TertiaryHue { get; set; }
    protected internal override double SurfaceHue { get; set; }
    protected internal override double SurfaceChroma { get; set; }

    public GeometricScheme(HCTA origin, ColorGeometry colorGeometry, TextStyleType textStyle, 
        SaturationType saturation, ToneGap darkLightGap, ToneGap onColorGap, ToneGap coreContainerGap,
        bool isDarkScheme = true) : base(origin)
    {
        ColorGeometry = colorGeometry;
        TextStyle = textStyle;
        Saturation = saturation;
        DarkLightGap = darkLightGap;
        OnColorGap = onColorGap;
        CoreContainerGap = coreContainerGap;
        
        SurfaceDifference = DifferenceFromSource.UseChromaOverride;
        SurfaceChroma = Math.Min(Origin!.C, 8);
        
        switch (ColorGeometry)
        {
            case ColorGeometry.Analogous:
                SecondaryDifference = DifferenceFromSource.HueShift;
                TertiaryDifference = DifferenceFromSource.NegativeHueShift | DifferenceFromSource.HueShift;
                break;
            case ColorGeometry.Complementary:
                SecondaryDifference = DifferenceFromSource.RelativeDesaturateLarge;
                TertiaryDifference = DifferenceFromSource.UseHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 180);
                break;
            case ColorGeometry.Triadic:
                SecondaryDifference = DifferenceFromSource.UseHueOverride;
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 120);
                TertiaryDifference = DifferenceFromSource.UseHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 120);
                break;
            case ColorGeometry.Tetradic:
                SecondaryDifference = DifferenceFromSource.UseHueOverride;
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 90);
                TertiaryDifference = DifferenceFromSource.UseHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 90);
                SurfaceDifference = DifferenceFromSource.UseHueOverride;
                SurfaceHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 180);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(colorGeometry));
        }
        
        if (isDarkScheme) SetDark();
        else SetLight();
    }

    protected override void PreConstruct()
    {
        SurfaceChroma = Math.Min(Origin!.C, 8);
        
        switch (ColorGeometry)
        {
            case ColorGeometry.Analogous:
                break;
            case ColorGeometry.Complementary:
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 180);
                break;
            case ColorGeometry.Triadic:
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 120);
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 120);
                break;
            case ColorGeometry.Tetradic:
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 90);
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 90);
                SurfaceHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 180);
                break;
        }
    }
}
