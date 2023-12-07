namespace MaterialDesign.Color.Colorspaces;

/// <summary>
/// Represents a color in the HSLA color model.
/// </summary>
public struct HSLA(float h, float s, float l, float a) : IHSL, IAlpha
{
    public float H { get; set; } = h;
    public float S { get; set; } = s;
    public float L { get; set; } = l;
    public float A { get; set; } = a;
    public byte A255 => IAlpha.CalculateA255(A);

    /// <summary>
    /// Converts the current color to RGB representation.
    /// </summary>
    /// <returns>Returns an instance of IRGB representing the RGB representation of the color.</returns>
    public IRGB ToRGB() => ((Color)this).ToRGB();

    /// <summary>
    /// Inverts the color by converting it to the RGB color space, inverting the RGB channels,
    /// and then converting it back to the HSL color space.
    /// </summary>
    /// <returns>The inverted color in the HSL color space.</returns>
    public IHSL Invert() => ((((RGBA)ToRGB()).Invert() as Color) as IHSL)!;
    
    public static implicit operator Color(HSLA hsla) => new(hsla.H, hsla.S, hsla.L, hsla.A);
    public static implicit operator HSLA(Color color) => new(color.H, color.S, color.L, color.A);
}