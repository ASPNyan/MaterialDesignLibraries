using MaterialDesign.Color.Colorspaces;

namespace MaterialDesign.Color.Palettes;

/// <summary>
/// Represents a core palette used for color theming.
/// </summary>
public readonly struct CorePalette(HCTA hcta)
{
    /// <summary>
    /// Gets the <see cref="HCTA">Color</see> originally provided to the palette.
    /// </summary>
    public HCTA Origin { get; } = hcta;
    

    public TonalPalette Primary { get; } = new(hcta.H, Math.Max(hcta.C, 48));
    public TonalPalette PrimaryContent { get; } = new(hcta);
    public TonalPalette Secondary { get; } = new(hcta.H, 16);
    public TonalPalette SecondaryContent { get; } = new(hcta.H, hcta.C / 3);
    public TonalPalette Tertiary { get; } = new(hcta.H, 24);
    public TonalPalette TertiaryContent { get; } = new(hcta.H, hcta.C / 2);
    public TonalPalette Neutral { get; } = new(hcta.H, 4);
    public TonalPalette NeutralContent { get; } = new(hcta.H, Math.Min(hcta.C / 12, 4));
    public TonalPalette NeutralVariant { get; } = new(hcta.H, 8);
    public TonalPalette NeutralVariantContent { get; } = new(hcta.H, Math.Min(hcta.C / 6, 8));

    public const double ErrorHue = 25;
    public const double ErrorChroma = 84;
    
#pragma warning disable CA1822 // Providing the error palette like this just to comply with material norms
    // ReSharper disable once MemberCanBeMadeStatic.Global
    public TonalPalette Error => ErrorPalette;
#pragma warning restore CA1822
    public static TonalPalette ErrorPalette => new(ErrorHue, ErrorChroma);
}