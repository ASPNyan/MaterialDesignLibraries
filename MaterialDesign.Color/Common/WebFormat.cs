namespace MaterialDesign.Color.Common;

public readonly struct WebFormat
{
    public string Format { get; }
    public string? Option { get; }

    private WebFormat(string format, string? option = null)
    {
        Format = format;
        Option = option;
    }

    public const string HTML = nameof(HTML);
    public const string Closed = nameof(Closed);
    public const string XML = nameof(XML);
    
    public const string JSON = nameof(JSON);

    public const string PlainText = nameof(PlainText);
    
    public const string CSS = nameof(CSS);
    public const string RGBA = nameof(RGBA);
    public const string HSLA = nameof(HSLA);
    public const string Hex = nameof(Hex);
    public const string Px = nameof(Px);
    public const string Vw = nameof(Vw);
    public const string Vh = nameof(Vh);
    public const string VMax = nameof(VMax);
    public const string VMin = nameof(VMin);
    public const string Percent = nameof(Percent);
    
    public static WebFormat AsCSS(string value) => new(CSS, value);
    public static WebFormat AsHTML(bool closed = false) => new(HTML, closed ? Closed : null);
    public static WebFormat AsXML(bool closed = true) => new(XML, closed ? Closed : null);
    public static WebFormat AsJSON() => new(JSON);
    public static WebFormat AsPlainText() => new(PlainText);
}