using MaterialDesign.Web.Fonts.Enums;

namespace MaterialDesign.Web.Fonts;

public class FontSourceBuilder
{
    private List<string> Sources { get; } = [];

    public FontSourceBuilder AddLocalSource(string fontName, bool quoted = true)
    {
        Sources.Add(quoted ? $"""local("{fontName}")""" : $"local({fontName})");
        return this;
    }

    public FontSourceBuilder AddUrlSource(string fontAddress, FontFormat? format = null, FontTechnology? tech = null,
        bool quoted = true)
    {
        string value = quoted ? $"""url("{fontAddress}")""" : $"url({fontAddress})";
        if (format is not null) value += $" format({format})";
        if (tech is not null) value += $" tech({GetTechString()}";
        
        Sources.Add(value);

        return this;

        string GetTechString()
        {
            return tech switch
            {
                // ReSharper disable StringLiteralTypo
                FontTechnology.ColorCBDT => "color-cbdt",
                FontTechnology.ColorCOLRV0 => "color-colrv0",
                FontTechnology.ColorCOLRV1 => "color-colrv1",
                FontTechnology.ColorSBIX => "color-sbix",
                // ReSharper restore StringLiteralTypo
                FontTechnology.ColorSvg => "color-svg",
                FontTechnology.FeaturesAAT => "features-aat",
                FontTechnology.FeaturesGraphite => "features-graphite",
                FontTechnology.FeaturesOpenType => "features-opentype",
                FontTechnology.Incremental => "incremental",
                FontTechnology.Palettes => "palettes",
                FontTechnology.Variations => "variations",
                null => string.Empty,
                _ => throw new ArgumentOutOfRangeException(nameof(tech), tech, null)
            };
        }
    }

    public override string ToString()
    {
        if (Sources is []) throw new InvalidOperationException("Font Sources must contain at least one source.");
        if (Sources.Count is 1) return $"src: {Sources.First()}";
        string sourceString = Sources.Aggregate("src: \n", (current, source) => current + $"    {source},\n");

        return sourceString.TrimEnd(['\n', ',']) + ';';
    }
}