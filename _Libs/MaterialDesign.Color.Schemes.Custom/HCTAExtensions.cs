using System.Diagnostics.Contracts;

namespace MaterialDesign.Color.Schemes.Custom;

public static class HCTAExtensions
{
    /// <summary>
    /// Calculates the chroma distance ratio between the provided color and the color with no chroma.
    /// </summary>
    /// <returns>A <see cref="Double"/> value between 0 and 100, where 100 means the chroma is at its max value</returns>
    public static double GetChromaDistanceRatio(this HCTA hcta)
    {
        double divisor = CalculateDistanceValue(hcta.ExactMaxChroma(3));
        
        return Math.Clamp(CalculateDistanceValue(hcta.C) / divisor * 100, 0, 100);
    }

    private static double CalculateDistanceValue(double chroma)
    {
        // polynomial 4th degree function is y = 7e-5x^4 + 0.01702x^3 + 0.09963x^2 + 2.8325x - 4.98696

        const double fifthDegree = -7e-5;
        const double fourthDegree = 0.01702;
        const double thirdDegree = 0.09963;
        const double secondDegree = 2.8325;
        const double firstDegree = 4.98696;
        
        double c4 = Math.Pow(chroma, 4);
        double c3 = Math.Pow(chroma, 3);
        double c2 = Math.Pow(chroma, 2);
        
        return fifthDegree * c4 + fourthDegree * c3 + thirdDegree * c2 + secondDegree * chroma - firstDegree;
    }

    [Pure]
    public static HCTA CreateFromChromaDistance(this HCTA hcta, double dist)
    {
        throw new NotImplementedException("Implement.");
    }

    private static double InvertChromaDistance(double chroma)
    {
        throw new NotImplementedException("Implement.");
    }
}