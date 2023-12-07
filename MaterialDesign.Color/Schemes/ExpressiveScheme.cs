using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Schemes;

public record ExpressiveScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Expressive, ContrastLevel, IsDark,
        Primary: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H + 240), 40),
        Secondary: new TonalPalette(GetRotatedHue(Source, Hues, SecondaryRotations), 24),
        Tertiary: new TonalPalette(GetRotatedHue(Source, Hues, TertiaryRotations), 32),
        Neutral: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H + 15), 8),
        NeutralVariant: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H + 15), 12))
{
    private static readonly List<double> Hues = [0, 21, 51, 121, 151, 191, 271, 321, 360];

    private static readonly List<double> SecondaryRotations = [45, 95, 45, 20, 45, 90, 45, 45, 45];

    private static readonly List<double> TertiaryRotations = [120, 120, 20, 45, 20, 15, 20, 120, 120];
}