using MaterialDesign.Disliked;
using MaterialDesign.Temperature;

namespace MaterialDesign.Schemes;

public record ContentScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Content, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, Source.C),
        Secondary: new TonalPalette(Source.H, Math.Max(Source.C - 32, Source.C / 2)),
        Tertiary: new TonalPalette(new TemperatureCache(Source).GetAnalogousColors(3, 6)[2].FixIfDisliked()),
        Neutral: new TonalPalette(Source.H, Source.C / 8),
        NeutralVariant: new TonalPalette(Source.H, Source.C / 8 + 4));