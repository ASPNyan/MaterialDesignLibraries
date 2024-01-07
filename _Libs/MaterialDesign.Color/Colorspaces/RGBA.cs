namespace MaterialDesign.Color.Colorspaces;

public struct RGBA(byte r, byte g, byte b, float a = 100f) : IRGB, IAlpha, IEquatable<RGBA>
{
    public byte R { get; set; } = r;
    public byte G { get; set; } = g;
    public byte B { get; set; } = b;
    public float A { get; set; } = a;
    public byte A255 => IAlpha.CalculateA255(A);

    /// <summary>
    /// Converts the RGB values into an int, represented by: <c>RRRR_RRRR_GGGG_GGGG_BBBB_BBBB_AAAA_AAAA</c>.
    /// This conversion is lossy if your color was created with HSL, since using RGB for HSL implementations is lossy
    /// by implementation. The alpha is also converted to a byte, which is also lossy.
    /// </summary>
    
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

    
    public static RGBA FromUIntRepresentation(uint rep)
    {
        float a = (byte)rep / 255f * 100;
        rep >>= 8;
        byte b = (byte)rep;
        rep >>= 8;
        byte g = (byte)rep;
        rep >>= 8;
        byte r = (byte)rep;
        return new RGBA(r, g, b, a);
    }
    
    public static implicit operator Color(RGBA rgba) => new(rgba.R, rgba.G, rgba.B, rgba.A);
    public static implicit operator RGBA(Color color) => new(color.R, color.G, color.B, color.A);
    
    
    public static explicit operator uint(RGBA color)
        => color.ToUIntRepresentation();

    
    public static explicit operator RGBA(uint rep)
        => FromUIntRepresentation(rep);

    /// <summary>
    /// A shorthand for the <see cref="Invert"/> method.
    /// </summary>
    /// <returns>The inverse of an RGB value</returns>
    public static RGBA operator ~(RGBA rgba) => (RGBA)rgba.Invert();

    /// <summary>
    /// Returns the inverse of an RGB value. The shorthand for this method is the ~ (bitwise NOT) operator.
    /// </summary>
    /// <returns>The inverse of an RGB value</returns>
    public IRGB Invert() => new RGBA((byte)~R, (byte)~G, (byte)~B);

    public bool Equals(RGBA other) => ToUIntRepresentation() == other.ToUIntRepresentation();

    public override bool Equals(object? obj) => obj is RGBA rgba && Equals(rgba);

    public override int GetHashCode() => HashCode.Combine(R, G, B, A);

    public static bool operator ==(RGBA left, RGBA right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(RGBA left, RGBA right)
    {
        return !left.Equals(right);
    }

    /// <summary>
    /// Returns a CSS <c>rgba</c> function.
    /// </summary>
    /// <returns>A CSS <c>rgba</c> function</returns>
    public override string ToString() => $"rgba({R}, {G}, {B}, {A/100:G5})";
}