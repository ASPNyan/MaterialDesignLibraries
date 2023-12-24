namespace MaterialDesign.Color.Schemes;

public record RainbowScheme(HCTA Source, bool IsDark)
    : DynamicScheme(Source, Variant.Rainbow, IsDark,
        Primary: new TonalPalette(Source.H, 48),
        Secondary: new TonalPalette(Source.H, 16),
        Tertiary: new TonalPalette(Colorspaces.Color.SanitizeDegrees(Source.H + 60), 24),
        Neutral: new TonalPalette(Source.H, 0),
        NeutralVariant: new TonalPalette(Source.H, 0));