namespace MaterialDesign.Schemes;

public record NeutralScheme(HCTA Source, bool IsDark, double ContrastLevel = 0)
    : DynamicScheme(Source, Variant.Neutral, ContrastLevel, IsDark,
        Primary: new TonalPalette(Source.H, 12),
        Secondary: new TonalPalette(Source.H, 8),
        Tertiary: new TonalPalette(Source.H, 16),
        Neutral: new TonalPalette(Source.H, 2),
        NeutralVariant: new TonalPalette(Source.H, 2));