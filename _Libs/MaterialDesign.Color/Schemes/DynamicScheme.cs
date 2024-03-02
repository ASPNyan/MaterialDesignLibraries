namespace MaterialDesign.Color.Schemes;

/// <summary>
/// Represents a dynamic scheme.
/// </summary>
/// <param name="Source">The source color of the theme as an HCTA.</param>
/// <param name="Variant">The variant, or style, of the theme.</param>
/// <param name="IsDark">Whether the scheme is in dark mode or light mode.</param>
/// <param name="Primary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually colorful.</param>
/// <param name="Secondary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually less colorful.</param>
/// <param name="Tertiary">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually a different hue from primary and colorful.</param>
/// <param name="Neutral">Given a tone, this palette produces a color. Hue and chroma of
/// the color are specified in the design specification of the variant. Usually not colorful at all, intended for background &amp; surface colors.</param>
/// <param name="NeutralVariant">Given a tone, this palette produces a color. Hue and chroma
/// of the color are specified in the design specification of the variant.
/// Usually not colorful, but slightly more colorful than Neutral. Intended for backgrounds &amp; surfaces.</param>
public abstract record DynamicScheme(HCTA Source, Variant Variant, bool IsDark, TonalPalette Primary,
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

    public static DynamicScheme Create(CorePalette palette, Variant variant, bool isDark = true)
    {
        HCTA source = palette.Origin;

        return Create(source, variant, isDark);
    }
    
    public static DynamicScheme Create(HCTA source, Variant variant, bool isDark = true)
    {
        return variant switch
        {
            Variant.Monochrome => new MonochromeScheme(source, isDark),
            Variant.Neutral => new NeutralScheme(source, isDark),
            Variant.TonalSpot => new TonalSpotScheme(source, isDark),
            Variant.Vibrant => new VibrantScheme(source, isDark),
            Variant.Expressive => new ExpressiveScheme(source, isDark),
            Variant.Fidelity => new FidelityScheme(source, isDark),
            Variant.Content => new ContentScheme(source, isDark),
            Variant.Rainbow => new RainbowScheme(source, isDark),
            Variant.FruitSalad => new FruitSaladScheme(source, isDark),
            _ => throw new ArgumentOutOfRangeException(nameof(variant), variant, null)
        };
    }

    public static TScheme Create<TScheme>(HCTA source, bool isDark = true) where TScheme : DynamicScheme
        => (TScheme)Create(source, typeof(TScheme), isDark);

    public static DynamicScheme Create(HCTA source, Type custom, bool isDark = true)
    {
        if (CustomTypes.TryGetValue(custom, out Func<HCTA, bool, DynamicScheme>? constructor)) 
            return constructor(source, isDark);

        throw new KeyNotFoundException("Custom types cannot be constructed with Create without calling " +
                                       "AddSelfAsCustomScheme in the static constructor.");
    }

    private static readonly Dictionary<Type, Func<HCTA, bool, DynamicScheme>> CustomTypes = new();
    
    /// <summary>
    /// Adds itself to a collection of constructors sorted by type to allow creation of custom schemes with the
    /// <see cref="Create{TScheme}"/> and <see cref="Create(MaterialDesign.Color.Colorspaces.HCTA,Type,bool)"/>
    /// methods. Should be used in static constructors to add itself as soon as possible.
    /// </summary>
    /// <param name="constructor">The method to construct the scheme using a source <see cref="HCTA"/> and
    /// <see cref="Boolean"/>.</param>
    /// <typeparam name="TScheme">The type of the scheme. This should be the containing class.</typeparam>
    protected static void AddSelfAsCustomScheme<TScheme>(Func<HCTA, bool, TScheme> constructor) 
        where TScheme : DynamicScheme => 
            CustomTypes.TryAdd(typeof(TScheme), constructor);
}