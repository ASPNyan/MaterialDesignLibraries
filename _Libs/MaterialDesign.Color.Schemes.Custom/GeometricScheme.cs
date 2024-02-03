namespace MaterialDesign.Color.Schemes.Custom;

public sealed record GeometricScheme : CustomSchemeBase
{
    public ColorGeometry ColorGeometry { get; }

    protected override TextStyleType TextStyle { get; }
    protected override SaturationType Saturation { get; }
    protected override ToneGap DarkLightGap { get; }
    protected override ToneGap OnColorGap { get; }
    protected override ToneGap CoreContainerGap { get; }
    protected override DifferenceFromSource PrimaryDifference => DifferenceFromSource.None;
    protected override DifferenceFromSource SecondaryDifference { get; }
    protected override DifferenceFromSource TertiaryDifference { get; }
    protected override DifferenceFromSource SurfaceDifference { get; }

    protected override double SecondaryHue { get; set; }
    protected override double TertiaryHue { get; set; }
    protected override double SurfaceHue { get; set; }
    protected override double SurfaceChroma { get; set; }

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
        
        SurfaceDifference = DifferenceFromSource.UseSurfaceChromaOverride;
        SurfaceChroma = Math.Min(Origin!.C, 8);
        
        switch (ColorGeometry)
        {
            case ColorGeometry.Analogous:
                SecondaryDifference = DifferenceFromSource.HueShift;
                TertiaryDifference = DifferenceFromSource.NegativeHueShift | DifferenceFromSource.HueShift;
                break;
            case ColorGeometry.Complementary:
                SecondaryDifference = DifferenceFromSource.RelativeDesaturateLarge;
                TertiaryDifference = DifferenceFromSource.UseTertiaryHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 180);
                break;
            case ColorGeometry.Triadic:
                SecondaryDifference = DifferenceFromSource.UseSecondaryHueOverride;
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 120);
                TertiaryDifference = DifferenceFromSource.UseTertiaryHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 120);
                break;
            case ColorGeometry.Tetradic:
                SecondaryDifference = DifferenceFromSource.UseSecondaryHueOverride;
                SecondaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H + 90);
                TertiaryDifference = DifferenceFromSource.UseTertiaryHueOverride;
                TertiaryHue = Colorspaces.Color.SanitizeDegrees(Origin.H - 90);
                SurfaceDifference = DifferenceFromSource.UseSurfaceHueOverride;
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
