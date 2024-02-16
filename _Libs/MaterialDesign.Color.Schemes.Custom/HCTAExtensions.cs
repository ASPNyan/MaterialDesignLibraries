using System.Diagnostics.Contracts;
using static System.Math;

namespace MaterialDesign.Color.Schemes.Custom;

public static class HCTAExtensions
{
    private const double Divisor = 24666.56304;
    
    /// <summary>
    /// Calculates the chroma distance ratio between the provided color and the color with no chroma.
    /// </summary>
    /// <returns>A <see cref="Double"/> value between 0 and 100, where 100 means the chroma is at its max value</returns>
    public static double GetChromaDistanceRatio(this HCTA hcta)
    {
        double c = Clamp(hcta.C, 0, 150);
        
        return Clamp(CalculateDistanceValue(c) / Divisor * 100, 0, 100);
    }
    
    private const double A = 1.369;
    private const double B = 1.845;

    private static double CalculateDistanceValue(double x) => A * Pow(x, B);
    
    /// <summary>
    /// Creates a new HCTA color using an existing color's hue and tone with the provided chroma ratio
    /// generated from <see cref="GetChromaDistanceRatio"/>.
    /// </summary>
    /// <remarks>After some testing (no PCs were harmed), this should be accurate to 1.07e-14 of a chroma.</remarks>
    [Pure]
    public static HCTA CreateFromChromaDistanceRatio(this HCTA hcta, double ratio)
    {
        ratio = Clamp(ratio, 0, 100);
        
        double c = Pow(ratio / 100 * Divisor / A, 1 / B);
        
        return new HCTA(hcta.H, c, hcta.T);
    }
}