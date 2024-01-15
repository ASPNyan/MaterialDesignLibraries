namespace MaterialDesign.Web.Fonts;

public class FontFaceBuilder
{
    private string? _family;
    private FontWeight? _weight;
    private FontSourceBuilder? _source;

    public FontFaceBuilder FontFamily(string family, bool quoted = true)
    {
        _family = quoted ? '"' + family + '"' : family;
        return this;
    }

    public FontFaceBuilder FontWeight(FontWeight weight)
    {
        _weight = weight;
        return this;
    }

    public FontFaceBuilder FontSource(Action<FontSourceBuilder> sourceBuilder)
    {
        sourceBuilder(_source = new FontSourceBuilder());
        return this;
    }

    public FontFace Build() => new()
    {
        Family = _family ?? throw new InvalidOperationException("You may not create a font face without a font family name."),
        Weight = _weight?.ToString() ?? throw new InvalidOperationException("You may not create a font face without a font weight value."),
        SourceString = _source?.ToString() ?? throw new InvalidOperationException("You may not create a font face without a font source.")
    };
}