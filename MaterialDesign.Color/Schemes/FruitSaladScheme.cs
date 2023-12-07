using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Schemes;

public record FruitSaladScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.FruitSalad, ContrastLevel, IsDark,
        Primary: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H - 50), 48),
        Secondary: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H - 50), 36),
        Tertiary: new TonalPalette(Source.H, 36),
        Neutral: new TonalPalette(Source.H, 10),
        NeutralVariant: new TonalPalette(Source.H, 16));