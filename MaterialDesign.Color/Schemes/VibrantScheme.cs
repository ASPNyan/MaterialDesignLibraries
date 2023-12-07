using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Schemes;

public record VibrantScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Vibrant, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, 120),
        Secondary: new TonalPalette(GetRotatedHue(Source, Hues, SecondaryRotations), 24),
        Tertiary: new TonalPalette(GetRotatedHue(Source, Hues, TertiaryRotations), 32),
        Neutral: new TonalPalette(Source.H, 10),
        NeutralVariant: new TonalPalette(Source.H, 12))
{
    private static readonly List<double> Hues = [0, 41, 61, 101, 131, 181, 251, 301, 360];
    private static readonly List<double> SecondaryRotations = [18, 15, 10, 12, 15, 18, 15, 12, 12];
    private static readonly List<double> TertiaryRotations = [35, 30, 20, 25, 30, 35, 30, 25, 25];
}