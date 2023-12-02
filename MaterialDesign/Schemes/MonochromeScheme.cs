namespace MaterialDesign.Schemes;

public record MonochromeScheme(HCTA Source, bool IsDark, double ContrastLevel = 0) 
    : DynamicScheme(Source, Variant.Monochrome, ContrastLevel, IsDark, 
        Primary: new TonalPalette(Source.H, 0),
        Secondary: new TonalPalette(Source.H, 0),
        Tertiary: new TonalPalette(Source.H, 0),
        Neutral: new TonalPalette(Source.H, 0),
        NeutralVariant: new TonalPalette(Source.H, 0));