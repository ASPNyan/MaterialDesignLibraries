namespace MaterialDesign.Color.Schemes;

public record NeutralScheme(HCTA Source, bool IsDark)
    : DynamicScheme(Source, Variant.Neutral, IsDark,
        Primary: new TonalPalette(Source.H, 12),
        Secondary: new TonalPalette(Source.H, 8),
        Tertiary: new TonalPalette(Source.H, 16),
        Neutral: new TonalPalette(Source.H, 2),
        NeutralVariant: new TonalPalette(Source.H, 2));