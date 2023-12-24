namespace MaterialDesign.Color.Schemes;

public record MonochromeScheme(HCTA Source, bool IsDark) 
    : DynamicScheme(Source, Variant.Monochrome, IsDark, 
        Primary: new TonalPalette(Source.H, 0),
        Secondary: new TonalPalette(Source.H, 0),
        Tertiary: new TonalPalette(Source.H, 0),
        Neutral: new TonalPalette(Source.H, 0),
        NeutralVariant: new TonalPalette(Source.H, 0));