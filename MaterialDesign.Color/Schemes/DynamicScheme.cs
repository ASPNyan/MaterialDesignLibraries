using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Schemes;

/// <summary>
/// Represents a dynamic scheme.
/// </summary>
/// <param name="Source">The source color of the theme as an HCTA.</param>
/// <param name="Variant">The variant, or style, of the theme.</param>
/// <param name="ContrastLevel">Value from -1 to 1. -1 represents minimum contrast,
/// 0 represents standard (i.e. the design as specified), and 1 represents maximum contrast.</param>
/// <param name="IsDark">Whether the scheme is in dark mode or light mode.</param>
/// <param name="Primary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually colorful.</param>
/// <param name="Secondary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually less colorful.</param>
/// <param name="Tertiary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually a different hue from primary and colorful.</param>
/// <param name="Neutral">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually not colorful at all, intended for background & surface colors.</param>
/// <param name="NeutralVariant">Given a tone, this palette produces a color. Hue and chroma
/// of the color are specified in the design specification of the variant.
/// Usually not colorful, but slightly more colorful than Neutral. Intended for backgrounds & surfaces.</param>
public abstract record DynamicScheme(HCTA Source, Variant Variant, double ContrastLevel, bool IsDark, TonalPalette Primary,
    TonalPalette Secondary, TonalPalette Tertiary, TonalPalette Neutral, TonalPalette NeutralVariant)
{
    protected static double GetRotatedHue(HCTA source, List<double> hues, List<double> rotations)
    {
        double sourceHue = source.H;

        if (rotations.Count is 1) return Colorspaces.Color.SanitizeDegrees(sourceHue + rotations[0]);

        for (int i = 0; i <= hues.Count - 2; i++)
        {
            double thisHue = hues[i];
            double nextHue = hues[i + 1];
            if (thisHue < sourceHue && sourceHue < nextHue) return Colorspaces.Color.SanitizeDegrees(sourceHue + rotations[i]);
        }

        return sourceHue;
    }

    public static DynamicScheme Create(CorePalette palette, Variant variant, bool isDark = true, double contrastLevel = 0)
    {
        HCTA source = palette.Origin;
        

        return variant switch
        {
            Variant.Monochrome => new MonochromeScheme(source, isDark, contrastLevel),
            Variant.Neutral => new NeutralScheme(source, isDark, contrastLevel),
            Variant.TonalSpot => new TonalSpotScheme(source, isDark, contrastLevel),
            Variant.Vibrant => new VibrantScheme(source, isDark, contrastLevel),
            Variant.Expressive => new ExpressiveScheme(source, isDark, contrastLevel),
            Variant.Fidelity => new FidelityScheme(source, isDark, contrastLevel),
            Variant.Content => new ContentScheme(source, isDark, contrastLevel),
            Variant.Rainbow => new RainbowScheme(source, isDark, contrastLevel),
            Variant.FruitSalad => new FruitSaladScheme(source, isDark, contrastLevel),
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null)
        };
    }
    
    public static DynamicScheme Create(HCTA source, Variant variant, bool isDark = true, double contrastLevel = 0)
    {
        return variant switch
        {
            Variant.Monochrome => new MonochromeScheme(source, isDark, contrastLevel),
            Variant.Neutral => new NeutralScheme(source, isDark, contrastLevel),
            Variant.TonalSpot => new TonalSpotScheme(source, isDark, contrastLevel),
            Variant.Vibrant => new VibrantScheme(source, isDark, contrastLevel),
            Variant.Expressive => new ExpressiveScheme(source, isDark, contrastLevel),
            Variant.Fidelity => new FidelityScheme(source, isDark, contrastLevel),
            Variant.Content => new ContentScheme(source, isDark, contrastLevel),
            Variant.Rainbow => new RainbowScheme(source, isDark, contrastLevel),
            Variant.FruitSalad => new FruitSaladScheme(source, isDark, contrastLevel),
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null)
        };
    }
}