namespace MaterialDesign.Colorspaces;

public interface IRGB : IEquatable<IRGB>, IEquatable<Color>, IEquatable<System.Drawing.Color>
{
    /// <summary>
    /// The red value of the RGB color, 0-255
    /// </summary>
    public byte R { get; set; }
    /// <summary>
    /// The green value of the RGB color, 0-255
    /// </summary>
    public byte G { get; set; }
    /// <summary>
    /// The blue value of the RGB color, 0-255
    /// </summary>
    public byte B { get; set; }

    /// <summary>
    /// Returns the inverse of an RGB value. The shorthand for this method is the ~ (bitwise NOT) operator.
    /// </summary>
    /// <returns>The inverse of an RGB value</returns>
    public IRGB Invert();
    
    bool IEquatable<IRGB>.Equals(IRGB? rgba)
    {
        return rgba is not null
               && R == rgba.R
               && G == rgba.G
               && B == rgba.B;
    }

    bool IEquatable<Color>.Equals(Color? color)
    {
        return color is not null
               && R == color.R
               && G == color.G
               && B == color.B;
    }

    bool IEquatable<System.Drawing.Color>.Equals(System.Drawing.Color color)
    {
        return R == color.R 
               && G == color.G
               && B == color.B;
    }
}