namespace MaterialDesign.Color.Colorspaces;

public interface IHSL : IEquatable<IHSL>, IEquatable<Color>, IEquatable<System.Drawing.Color>
{
    /// <summary>
    /// The hue of the HSL color, 0-360
    /// </summary>
    public float H { get; set; }
    /// <summary>
    /// The saturation of the HSL color, 0-100
    /// </summary>
    public float S { get; set; }
    /// <summary>
    /// The lightness (aka brightness) of the HSL color, 0-100
    /// </summary>
    public float L { get; set; }

    /// <summary>
    /// Converts the current object to the RGB color model.
    /// </summary>
    /// <returns>An object implementing the IRGB interface representing the equivalent RGB color.</returns>
    public IRGB ToRGB();

    /// <summary>
    /// Inverts the color.
    /// </summary>
    /// <returns>The inverted color in the form of an object implementing the IHSL interface.</returns>
    public IHSL Invert();

    bool IEquatable<IHSL>.Equals(IHSL? hsla)
    {
        return hsla is not null
               && Math.Abs(H - hsla.H) < 1e-5
               && Math.Abs(S - hsla.S) < 1e-5
               && Math.Abs(L - hsla.L) < 1e-5;
    }

    bool IEquatable<Color>.Equals(Color? color)
    {
        return color is not null
               && Math.Abs(H - color.H) < 1e-5
               && Math.Abs(S - color.S) < 1e-5
               && Math.Abs(L - color.L) < 1e-5;
    }

    bool IEquatable<System.Drawing.Color>.Equals(System.Drawing.Color color)
    {
        return Math.Abs(H - color.GetHue()) < 1e-5
               && Math.Abs(S - color.GetSaturation()) < 1e-5
               && Math.Abs(L - color.GetBrightness()) < 1e-5;
    }
}