namespace MaterialDesign.Schemes;

public record RainbowScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Rainbow, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, 48),
        Secondary: new TonalPalette(Source.H, 16),
        Tertiary: new TonalPalette(Color.SanitizeDegrees(Source.H + 60), 24),
        Neutral: new TonalPalette(Source.H, 0),
        NeutralVariant: new TonalPalette(Source.H, 0));