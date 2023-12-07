using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Schemes;

public record TonalSpotScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.TonalSpot, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, 36),
        Secondary: new TonalPalette(Source.H, 16),
        Tertiary: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H + 60), 24),
        Neutral: new TonalPalette(Source.H, 6),
        NeutralVariant: new TonalPalette(Source.H, 8));