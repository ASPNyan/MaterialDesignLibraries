namespace MaterialDesign.Web.Fonts.Enums;

public enum FontFormat
{
    /// <summary>
    /// OpenType Collection. Common extensions are <c>.otc</c> and <c>.ttc</c>
    /// </summary>
    Collection,
    /// <summary>
    /// Embedded OpenType font. Common extension is <c>.eot</c>
    /// </summary>
    EmbeddedOpenType,
    /// <summary>
    /// OpenType font. Common extensions are <c>.otf</c> and <c>.ttf</c>
    /// </summary>
    OpenType,
    /// <summary>
    /// SVG glyph font. Deprecated in major browsers since 2014 (except Safari which ended support in early 2023.)
    /// Common extensions are <c>.svg</c> and <c>.svgz</c>
    /// </summary>
    [Obsolete("SVG Fonts have not been supported in major browsers since 2014 (except Safari which ended support in early 2023.)")]
    Svg,
    /// <summary>
    /// TrueType font. Common extension is <c>.ttf</c>
    /// </summary>
    TrueType,
    /// <summary>
    /// WOFF 1.0 font, not to be confused with <see cref="Woff2">WOFF 2.0</see>. Common extension is <c>.woff</c>
    /// </summary>
    Woff,
    /// <summary>
    /// WOFF 2.0 font, not to be confused with <see cref="Woff">WOFF 1.0</see>. Common extension is <c>.woff</c>
    /// </summary>
    Woff2
}