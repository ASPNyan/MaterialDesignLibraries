using System.Text.Json;

namespace MaterialDesign.Color.Colorspaces;

/// <summary>
/// Color is a thread-safe, RGB and HSL (*not* HSV, they are two different things) based color container.
/// Technically, HSL represents an infinite amount of colours, but in practice (due to floating point storage and
/// display limitations) this color can store a total of <c>2^8 * 2^8 * 2^8</c> (or 16,777,216) different colours.
/// This is what screens are limited to anyway. Alpha is also supported, however it is represented differently to
/// <see cref="System.Drawing.Color"/> as it is a float and read from 0-100 instead of a byte from 0-255. To access
/// a byte implementation of the alpha, there is the get-only <see cref="A255"/>.
/// </summary>
[CLSCompliant(true)]
public class Color : IRGB, IHSL, IAlpha, IWebFormattable<Color>
{
    #region Class Implementation
    private readonly object _ioLock = new();
    private readonly Guid _hashCode = Guid.NewGuid();
    
    #region Value Storage
    
    private byte _r;
    private byte _g;
    private byte _b;

    /// <summary>
    /// The red value of the RGB color, 0-255
    /// </summary>
    public byte R
    {
        get { lock (_ioLock) return _r; }
        set 
        {
            lock (_ioLock)
            {
                _r = value;
                OnRGBUpdate();
            }
        }
    }

    /// <summary>
    /// The green value of the RGB color, 0-255
    /// </summary>
    public byte G
    {
        get { lock (_ioLock) return _g; }
        set 
        {
            lock (_ioLock)
            {
                _g = value;
                OnRGBUpdate();
            }
        }
    }

    /// <summary>
    /// The blue value of the RGB color, 0-255
    /// </summary>
    public byte B
    {
        get { lock (_ioLock) return _b; }
        set 
        {
            lock (_ioLock)
            {
                _b = value;
                OnRGBUpdate();
            }
        }
    }

    private void OnRGBUpdate()
    {
        // normalise RGB values to 0-1
        float r1 = _r / 255f;
        float g1 = _g / 255f;
        float b1 = _b / 255f;

        float cMax = Max3(r1, g1, b1);
        float cMin = Min3(r1, g1, b1);
        float delta = cMax - cMin; // delta is the difference between cMax and cMin

        _h = 0.0f;

        if (MathF.Abs(delta) >= 1e-5)
        {
            if (Math.Abs(cMax - r1) < 1e-5) _h = (g1 - b1) / delta;
            else if (Math.Abs(cMax - g1) < 1e-5) _h = 2f + (b1 - r1) / delta;
            else _h = 4f + (r1 - g1) / delta;
            
            _h *= 60;
            
            if (_h < 0) _h += 360;
        }
        
        _l = (cMax + cMin) / 2;

        _s = cMax < 1e-5 ? 0 : delta / (1 - Math.Abs(2 * _l - 1));

        _l *= 100;
        _s *= 100;
    }

    // private stored values
    private float _h;
    private float _s;
    private float _l;
    
    /// <summary>
    /// The hue of the HSL color, 0-360
    /// </summary>
    public float H
    {
        get { lock (_ioLock) return _h; }
        set 
        {
            lock (_ioLock)
            {
                _h = SanitizeDegrees(value);
                OnHSLUpdate();
            }
        }
    }

    /// <summary>
    /// The saturation of the HSL color, 0-100
    /// </summary>
    public float S
    {
        get { lock (_ioLock) return _s; }
        set 
        {
            lock (_ioLock)
            {
                _s = Math.Clamp(value, 0, 100);
                OnHSLUpdate();
            }
        }
    }

    /// <summary>
    /// The lightness (aka brightness) of the HSL color, 0-100
    /// </summary>
    public float L
    {
        get { lock (_ioLock) return _l; }
        set 
        {
            lock (_ioLock)
            {
                _l = Math.Clamp(value, 0, 100);
                OnHSLUpdate();
            }
        }
    }

    private void OnHSLUpdate()
    {
        // first, the equation requires sat and lit normalised to the range: 0 <= x <= 1
        // because they are percentages (0-100), just divide by 100
        float normSat = _s / 100;
        float normLit = _l / 100;
        // hue does not need to be normalised to any amount, it is of range: 0 <= x < 360
        // however because it cannot be 360, 360 must be converted to 0. sat and val should be checked for > 1 and < 0
        if (normSat > 1) normSat = 1;
        if (normLit > 1) normLit = 1;
        if (normSat < 0) normSat = 0;
        if (normLit < 0) normLit = 0;

        // now that values are normalised and within safe bounds, the actual formula can be applied.
        // but first, optimisation check
        if (normSat is 0)
        {
            _r = _g = _b = (byte)Round0(255 * normLit);
        }
        
        float chroma = (1 - Math.Abs(2 * normLit - 1)) * normSat;
        // from what I can tell online, this X variable is unlabelled. if it can be, please link the source 
        float x = chroma * (1 - Math.Abs(_h / 60 % 2 - 1));
        // I also can't find a label for this m variable either.
        float m = normLit - chroma / 2;

        (float r1, float g1, float b1) = _h switch
        {
            < 60 => (chroma, x, 0f),
            < 120 => (x, chroma, 0f),
            < 180 => (0f, chroma, x),
            < 240 => (0f, x, chroma),
            < 300 => (x, 0f, chroma),
            _ => (chroma, 0f, x)
        };

        _r = CalculateFullValue(r1);
        _g = CalculateFullValue(g1);
        _b = CalculateFullValue(b1);

        return;

        byte CalculateFullValue(float value) => (byte)Round0((value + m) * 255);
    }
    
    private float _a = 100;

    /// <summary>
    /// The alpha (transparency) representation of the color in either format when used in CSS. Ranges from 0-100.
    /// </summary>
    public float A
    {
        get => _a;
        set => _a = Math.Clamp(value, 0, 100);
    }

    /// <summary>
    /// The alpha (transparency) representation of the color in RGB format. Ranges from 0-255.
    /// </summary>
    public byte A255 => IAlpha.CalculateA255(A);
    
    #endregion
    
    #region Constructors

    public Color(byte r, byte g, byte b)
    {
        _r = r;
        _g = g;
        _b = b;

        OnRGBUpdate();
    }

    public Color(float h, float s, float l)
    {
        _h = h;
        _s = s;
        _l = l;

        OnHSLUpdate();
    }
    
    public Color(byte r, byte g, byte b, float a)
    {
        _r = r;
        _g = g;
        _b = b;
        A = a;

        OnRGBUpdate();
    }

    public Color(byte r, byte g, byte b, byte a) : this(r, g, b, a / 255f * 100)
    {
    }

    public Color(IRGB rgb) : this(rgb.R, rgb.G, rgb.B)
    {
    }
    
    public Color(float h, float s, float l, float a)
    {
        _h = h;
        _s = s;
        _l = l;
        A = a;

        OnHSLUpdate();
    }

    public Color(IAlpha color)
    {
        _a = color.A;
        
        switch (color)
        {
            case IRGB rgb:
                _r = rgb.R;
                _g = rgb.G;
                B = rgb.B;
                return;
            case IHSL hsl:
                _h = hsl.H;
                _s = hsl.S;
                L = hsl.L;
                return;
            default:
                throw new ArgumentException("Color must also be an RGB or HSL color", nameof(color));
        }
    }

    
    #endregion

    public Color Copy() => new(R, G, B, A);

    #region Conversions

    public IRGB ToRGB() => this;

    /// <summary>
    /// Converts the color to a hexadecimal-style string
    /// </summary>
    /// <param name="upper">Whether the letters in the hexadecimal data should be upper-case, true by default</param>
    /// <returns>
    /// The hexadecimal interpretation of the RGBA values as a string, beginning with # and then 8 characters of data.
    /// </returns>
    public string ToHexString(bool upper = true) => upper
        ? $"#{Convert.ToHexString(new[] { R, G, B, A255 })}" 
        : $"#{Convert.ToHexString(new[] { R, G, B, A255 })}".ToLower();
    
    /// <summary>
    /// Returns a CSS-style RGB function string
    /// </summary>
    public string ToRGBAString() => $"rgba({R}, {G}, {B}, {A:G5}%)";
    
    /// <summary>
    /// Returns a CSS-style HSL function string, converted from HSL.
    /// </summary>
    /// <param name="round">Whether the values should be rounded to 0 decimal places. Useful for debugging/testing</param>
    public string ToHSLAString(bool round = false)
    {
        if (!round) return $"hsla({H:G10}, {S:G10}%, {L:G10}%, {A:G10})";

        int roundedH = Round0(H);
        byte roundedS = (byte)Round0(S);
        byte roundedL = (byte)Round0(L);
        byte roundedA = (byte)Round0(A);
        return $"hsla({roundedH}, {roundedS}%, {roundedL}%, {roundedA}%)";
    }
    
    [CLSCompliant(false)]
    public static implicit operator Color(System.Drawing.Color color) 
        => new(color.R, color.G, color.B, color.A / 255f * 100);
    
    [CLSCompliant(false)]
    public static implicit operator System.Drawing.Color(Color color) 
        => System.Drawing.Color.FromArgb(color.A255, color.R, color.G, color.B);

    [CLSCompliant(false)]
    public static explicit operator uint(Color color)
        => color.ToUIntRepresentation();

    [CLSCompliant(false)]
    public static explicit operator Color(uint rep)
        => FromUIntRepresentation(rep);
    
    public static explicit operator HCTA(Color self) => HCTA.FromRGBA(self);

    public static explicit operator Color(HCTA self) => self.ToRGBA();

    /// <summary>
    /// Converts the RGB values into an int, represented by: <c>RRRR_RRRR_GGGG_GGGG_BBBB_BBBB_AAAA_AAAA</c>.
    /// This conversion is lossy if your color was created with HSL, since using RGB for HSL implementations is lossy
    /// by implementation. The alpha is also converted to a byte, which is also lossy.
    /// </summary>
    [CLSCompliant(false)]
    public uint ToUIntRepresentation()
    {
        uint value = R;
        value <<= 8;
        value |= G;
        value <<= 8;
        value |= B;
        value <<= 8;
        value |= A255;
        return value;
    }

    [CLSCompliant(false)]
    public static Color FromUIntRepresentation(uint rep)
    {
        float a = (byte)rep / 255f * 100;
        rep >>= 8;
        byte b = (byte)rep;
        rep >>= 8;
        byte g = (byte)rep;
        rep >>= 8;
        byte r = (byte)rep;
        return new Color(r, g, b, a);
    }

    public string ToFormattedString(WebFormat format)
    {
        if (!ValidFormats.Contains(format))
            throw new FormatException($"{typeof(Color).FullName} does not support web format {format.Format}");
        return format.Format switch
        {
            WebFormat.XML => $"""<Color R="{R}" G="{G}" B="{B}" A="{A}" """
                             + format.Option is WebFormat.Closed
                ? "/>"
                : "></Color>",
            WebFormat.JSON => JsonSerializer.Serialize(new ColorJson
                                    {
                                        RGBA = new RGBAJson(R, G, B, A255),
                                        HSLA = new HSLAJson(H, S, L, A)
                                    }),
            WebFormat.PlainText => $"RGBA: {R}, {G}, {B}, {A255} | " +
                                   $"HSLA: {MathF.Round(H, 2)}, {MathF.Round(S, 2)}, " +
                                   $"{MathF.Round(L, 2)}, {MathF.Round(A, 2)}",
            WebFormat.CSS => format.Option switch
            {
                WebFormat.RGBA => ToRGBAString(),
                WebFormat.HSLA => ToHSLAString(),
                _ => ToHexString()
            },
            _ => throw new FormatException($"{typeof(Color).FullName} does not support web format {format.Format}")
        };
    }

    public static Color FromFormattedString(string value, WebFormat? format = null)
    {
        value = value.Trim();
        
        if (value.StartsWith('{')) // JSON
        {
            if (format?.Format is not null and not WebFormat.JSON) 
                throw new FormatException("Expected JSON format but input was not valid.");
            
            var json = JsonSerializer.Deserialize<ColorJson>(value);
            return new Color(json.HSLA.H, json.HSLA.S, json.HSLA.L, json.HSLA.A);
        }

        if (value.StartsWith("<Color")) // XML
        {
            if (format?.Format is not null and not WebFormat.XML) 
                throw new FormatException("Expected XML format but input was not valid.");
            
            string[] values = value.Split(' ')[1..5]; // should get R, G, B, and A attributes.
            byte r = byte.Parse(values[0].Split('"')[1]);
            byte g = byte.Parse(values[1].Split('"')[1]);
            byte b = byte.Parse(values[2].Split('"')[1]);
            byte a = byte.Parse(values[3].Split('"')[1]);
            
            return new Color(r, g, b, a);
        }

        if (value.StartsWith("rgb")) // rgb()
        {
            if (format?.Format is not null and not WebFormat.CSS && format.Value.Option is not WebFormat.RGBA)
                throw new FormatException("Expected CSS RGBA function but input was not valid.");
            
            value = new string(value.Skip(4).ToArray());
            value = value.Replace(")", null);
            string[] values = value.Split(",", StringSplitOptions.TrimEntries);
            byte r = byte.Parse(values[0]);
            byte g = byte.Parse(values[1]);
            byte b = byte.Parse(values[2]);
            
            return new Color(r, g, b);
        }
        
        if (value.StartsWith("rgba")) // rgba()
        {
            if (format?.Format is not null and not WebFormat.CSS && format.Value.Option is not WebFormat.RGBA)
                throw new FormatException("Expected CSS RGBA function but input was not valid.");
            
            value = new string(value.Skip(5).ToArray());
            value = value.Replace(")", null);
            string[] values = value.Split(",", StringSplitOptions.TrimEntries);
            float h = float.Parse(values[0]);
            float s = float.Parse(values[1]);
            float l = float.Parse(values[2]);
            float a = float.Parse(values[3]);
            
            return new Color(h, s, l, a);
        }

        if (value.StartsWith('#'))
        {
            if ((format?.Format is not null and not WebFormat.CSS && format.Value.Option is not WebFormat.Hex)
                || (value.Length != 7 && value.Length != 9))
                throw new FormatException("Expected Hex format but input was not valid.");
            
            value = value.Replace("#", null);
            
            byte[] values = Convert.FromHexString(value);
            byte r = values[0];
            byte g = values[1];
            byte b = values[2];
            byte? a = value.Length is 9 ? values[3] : null;

            return new Color(r, g, b, a ?? 255);
        }

        throw new FormatException("Expected a valid string format but could not find any.");
    }

    public WebFormat[] ValidFormats => new[]
    {
        WebFormat.AsXML(), WebFormat.AsXML(false),
        WebFormat.AsJSON(), WebFormat.AsPlainText(),
        WebFormat.AsCSS(WebFormat.RGBA),
        WebFormat.AsCSS(WebFormat.HSLA),
        WebFormat.AsCSS(WebFormat.Hex)
    };

    #endregion
    
    public bool Equals(Color? other)
    {
        if (other is null) return false;
        return ToUIntRepresentation() == other.ToUIntRepresentation();
    }

    public bool Equals(System.Drawing.Color other)
    {
        return ToUIntRepresentation() == ((Color)other).ToUIntRepresentation();
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Color);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_hashCode);
    }

    public static bool operator ==(Color? left, Color? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Color? left, Color? right)
    {
        return !Equals(left, right);
    }

    IHSL IHSL.Invert() => new HSLA(H, S, L, A).Invert();
    public IRGB Invert() => new RGBA(R, G, B, A).Invert();

    #endregion

    #region Unrelated Static Methods
    public static int Min3(int x, int y, int z) => Math.Min(x, Math.Min(y, z));
    public static float Min3(float x, float y, float z) => Math.Min(x, Math.Min(y, z));
    public static double Min3(double x, double y, double z) => Math.Min(x, Math.Min(y, z));
    public static int Max3(int x, int y, int z) => Math.Max(x, Math.Max(y, z));
    public static float Max3(float x, float y, float z) => Math.Max(x, Math.Max(y, z));
    public static double Max3(double x, double y, double z) => Math.Max(x, Math.Max(y, z));

    public static int Round0(double x) => (int)Math.Truncate(x) + (x % 1 < 0.5 ? 0 : 1);

    public static int SanitizeDegrees(int deg)
    {
        deg %= 360;
        if (deg < 0) deg += 360;
        return deg;
    }
    
    public static float SanitizeDegrees(float deg)
    {
        deg %= 360;
        if (deg < 0) deg += 360;
        return deg;
    }
    
    public static double SanitizeDegrees(double deg)
    {
        deg %= 360;
        if (deg < 0) deg += 360;
        return deg;
    }
    
    public static double DifferenceDegrees(double a, double b) => 180 - Math.Abs(Math.Abs(a - b) - 180);
    #endregion

    private readonly struct ColorJson
    {
        public required RGBAJson RGBA { get; init; }
        public required HSLAJson HSLA { get; init; }
    }

    private readonly struct RGBAJson(byte r, byte g, byte b, byte a)
    {
        public byte R { get; } = r;
        public byte G { get; } = g;
        public byte B { get; } = b;
        public byte A { get; } = a;
    }
    
    private readonly struct HSLAJson(float h, float s, float l, float a)
    {
        public float H { get; } = h;
        public float S { get; } = s;
        public float L { get; } = l;
        public float A { get; } = a;
    }

}
