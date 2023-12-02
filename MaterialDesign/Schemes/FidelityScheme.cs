using MaterialDesign.Disliked;
using MaterialDesign.Temperature;

namespace MaterialDesign.Schemes;

public record FidelityScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Fidelity, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, 40),
        Secondary: new TonalPalette(Source.H, Math.Max(Source.C - 32, Source.C * 0.5)),
        Tertiary: new TonalPalette(new TemperatureCache(Source).GetComplement().FixIfDisliked()),
        Neutral: new TonalPalette(Source.H, Source.C / 8),
        NeutralVariant: new TonalPalette(Source.H, Source.C / 8 + 4)); 