using System.Diagnostics.Contracts;
using static System.Math;

namespace MaterialDesign.Color.Schemes.Custom;

public static class HCTAExtensions
{
    static HCTAExtensions()
    {
        HCTA hcta = new(0, 0, 50);

        for (int i = 1; i <= 150; i++)
        {
            hcta.C = i;

            double ratio = hcta.GetChromaDistanceRatio();
            
            DistRatioToChroma.Add(ratio, i);
        }
    }
    
    /// <summary>
    /// Calculates the chroma distance ratio between the provided color and the color with no chroma.
    /// </summary>
    /// <returns>A <see cref="Double"/> value between 0 and 100, where 100 means the chroma is at its max value</returns>
    public static double GetChromaDistanceRatio(this HCTA hcta)
    {
        const double divisor = 24666.56304; // CalculateDistanceValue(150)
        
        return Clamp(CalculateDistanceValue(hcta.C) / divisor * 100, 0, 100);
    }

    private static double CalculateDistanceValue(double chroma)
    {
        // polynomial 4th degree function is y = 7e-5x^4 + 0.01702x^3 + 0.09963x^2 + 2.8325x - 4.98696

        const double fifthDegree = -7e-5;
        const double fourthDegree = 0.01702;
        const double thirdDegree = 0.09963;
        const double secondDegree = 2.8325;
        const double firstDegree = 4.98696;
        
        double c4 = Pow(chroma, 4);
        double c3 = Pow(chroma, 3);
        double c2 = Pow(chroma, 2);
        
        return fifthDegree * c4 + fourthDegree * c3 + thirdDegree * c2 + secondDegree * chroma - firstDegree;
    }

    private static readonly Dictionary<double, double> DistRatioToChroma = [];
    
    /// <summary>
    /// Creates a new instance of a <see cref="HCTA"/> with a chroma generated using a ratio created by
    /// <see cref="GetChromaDistanceRatio"/>.
    /// </summary>
    /// <remarks>
    /// This generation should be accurate to 0.1 of a chroma, but is sometimes off by a bit more.
    /// Nothing significant, however.
    /// </remarks>
    [Pure]
    public static HCTA CreateFromChromaDistanceRatio(this HCTA hcta, double ratio)
    {
        if (ratio is < 0 or > 100) throw new InvalidOperationException($"Ratio must be >= 0 and <= 100, got {ratio}");

        if (DistRatioToChroma.TryGetValue(ratio, out double c))
            return new HCTA(hcta.H, c, hcta.T);

        double lastRatio = 0;
        foreach (double currentRatio in DistRatioToChroma.Keys)
        {
            if (currentRatio < ratio)
            {
                lastRatio = currentRatio;
                continue;
            }

            double lowerChroma = DistRatioToChroma[lastRatio];
            double upperChroma = DistRatioToChroma[currentRatio];

            double diffC = Abs(upperChroma - lowerChroma); // shouldn't be negative but if it has a moment you never know
            double multi = (ratio - lastRatio) / (currentRatio - lastRatio);

            double chroma = lowerChroma + diffC * multi; // guesstimate a value between the chromas

            return new HCTA(hcta.H, chroma, hcta.T);
        }

        throw new Exception("Unable to find a pair of ratios that fit the criteria to get the chroma.");
    }
}