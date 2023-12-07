using MaterialDesign.Color.Colorspaces;

namespace MaterialDesign.Color.Quantize;

public static class LABPointProvider
{
    /// <inheritdoc cref="LAB.FromRGBA"/>
    public static LAB FromRGBA(RGBA rgba) => LAB.FromRGBA(rgba);
    /// <inheritdoc cref="LAB.ToRGBA"/>
    public static RGBA FromLAB(LAB lab) => lab.ToRGBA();

    /// <summary>
    /// Calculates the distance between two LAB colors using the Euclidean distance formula.
    /// </summary>
    /// <param name="from">The first LAB color.</param>
    /// <param name="to">The second LAB color.</param>
    /// <returns>The distance between the two LAB colors.</returns>
    public static double Distance(LAB from, LAB to)
    {
        double dL = from.L - to.L;
        double dA = from.A - to.A;
        double dB = from.B - to.B;
        
        return dL * dL + dA * dA + dB * dB;
    }
}