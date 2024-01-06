using MaterialDesign.Color.Palettes;
using MaterialDesign.Color.Schemes;

namespace MaterialDesign.Theming;

/// <summary>
/// A customized type of <see cref="DynamicScheme"/> that behaves similarly to what's seen at
/// https://m3.material.io/theme-builder#/custom
/// </summary>
/// <param name="Source"></param>
/// <param name="IsDark"></param>
public record ThemeScheme(HCTA Source, bool IsDark)
    : DynamicScheme(Source, Variant.Custom, IsDark,
        Primary: new TonalPalette(Source),
        Secondary: new TonalPalette(Source.H, 16),
        Tertiary: new TonalPalette(Color.Colorspaces.Color.SanitizeDegrees(Source.H + 60), 24),
        Neutral: new TonalPalette(Source.H, 4),
        NeutralVariant: new TonalPalette(Source.H, 8))
{
    static ThemeScheme()
    {
        AddSelfAsCustomScheme((source, isDark) => new ThemeScheme(source, isDark));
    }
}