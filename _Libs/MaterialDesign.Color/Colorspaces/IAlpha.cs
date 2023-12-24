namespace MaterialDesign.Color.Colorspaces;

public interface IAlpha
{
    /// <summary>
    /// The alpha (transparency) representation of the color. Ranges from 0-100.
    /// </summary>
    public float A { get; set; }
    /// <summary>
    /// The alpha (transparency) representation of the color in RGB format. Ranges from 0-255.
    /// </summary>
    public byte A255 { get; }

    /// <summary>
    /// Calculates the byte representation of a given float value according to the formula: (a / 100) * 255
    /// </summary>
    /// <param name="a">The float value to be converted</param>
    /// <returns>The byte representation of the float value scaled to the range of 0-255</returns>
    public static byte CalculateA255(float a) => (byte)Color.Round0(a / 100 * 255);
}