using MaterialDesign.Color.Colorspaces;
using MaterialDesign.Color.Common;
using static System.Math;

namespace MaterialDesign.Color.Score;

/// <summary>
/// Given a large set of colors, remove colors that are unsuitable for a UI
/// theme, and rank the rest based on suitability.
///
/// Enables use of a high cluster count for image quantization, thus ensuring
/// colors aren't muddied, while curating the high cluster count to a much
/// smaller number of appropriate choices.
/// </summary>
public static class Score
{
    private const double TargetChroma = 48.0;
    private const double WeightProportion = 0.6;
    private const double WeightChromaAbove = 0.3;
    private const double WeightChromaBelow = 0.1;
    private const double CutoffChroma = 5.0;
    private const double CutoffExcitedProportion = 0.01;
    private const double WeightAverageProximity = 0.2;

    /// <summary>
    /// Given a map with keys of colors and values of how often the color appears,
    /// rank the colors based on suitability for being used for a UI theme.
    /// </summary>
    /// <param name="colorPopulations"><see cref="FrequencyMap{TValue}"/> with keys of colors and values of how often
    /// the color appears, usually from a source image.</param>
    /// <param name="desiredCount">The max count of the colors returned.</param>
    /// <returns>Colors sorted by suitability for a UI theme. The most suitable
    /// color is the first item, the least suitable is the last. There will
    /// always be at least one color returned. If all the input colors
    /// were not suitable for a theme, a default fallback color will be
    /// provided as Google Blue.</returns>
    public static FrequencyMap<HCTA, double> Scored(FrequencyMap<HCTA> colorPopulations, int desiredCount)
    {
        int avgR = 0;
        int avgG = 0;
        int avgB = 0;
        
        int[] huePopulation = new int[360];
        foreach ((HCTA hcta, int frequency) in colorPopulations)
        {
            int hue = (int)hcta.H;
            huePopulation[hue] += frequency;
            RGBA rgba = hcta.ToRGBA();
            avgR += rgba.R * frequency;
            avgG += rgba.G * frequency;
            avgB += rgba.B * frequency;
        }

        avgR /= colorPopulations.FrequencySum;
        avgG /= colorPopulations.FrequencySum;
        avgB /= colorPopulations.FrequencySum;

        LAB avgLAB = LAB.FromRGBA(new RGBA((byte)avgR, (byte)avgG, (byte)avgB));
        
        double[] hueExcitedProportions = new double[360];
        for (int hue = 0; hue < 360; hue++)
        {
            double proportion = huePopulation[hue] / (double)colorPopulations.FrequencySum;
            for (int i = hue - 14; i < hue + 16; i++)
            {
                int neighbor = Colorspaces.Color.SanitizeDegrees(i);
                hueExcitedProportions[neighbor] += proportion;
            }
        }

        FrequencyMap<HCTA, double> scored = new();
        foreach (HCTA hcta in colorPopulations.Values)
        {
            int hue = (int)hcta.H;
            double proportion = hueExcitedProportions[hue];

            if (hcta.C < CutoffChroma || proportion <= CutoffExcitedProportion) continue;

            double proportionScore = proportion * 100 * WeightProportion;
            double chromaWeight = hcta.C < TargetChroma ? WeightChromaBelow : WeightChromaAbove;
            double chromaScore = (hcta.C - TargetChroma) * chromaWeight;
            double avgProximityScore = (100 - CalculateCIEDE2000Distance(avgLAB, LAB.FromRGBA(hcta.ToRGBA()))) * WeightAverageProximity;
            double score = proportionScore + chromaScore + avgProximityScore;
            scored.Add(hcta, score);
        }

        FrequencyMap<HCTA, double> chosenColors = new();
        for (int differenceDegrees = 90; differenceDegrees >= 15; differenceDegrees--)
        {
            chosenColors.Clear();
            foreach ((HCTA hcta, double score) in scored.GetMostFrequentWithFrequencies(scored.Count))
            {
                bool isDuplicate = chosenColors.Any(kvp => Colorspaces.Color.SanitizeDegrees(kvp.Key.H - hcta.H) < differenceDegrees);

                if (!isDuplicate)
                {
                    chosenColors.Add(hcta, score);
                    if (chosenColors.Count >= desiredCount) break;
                }
            }
            if (chosenColors.Count >= desiredCount) break;
        }

        if (chosenColors.Count is 0) chosenColors.Add(HCTA.FromRGBA((RGBA)0x4285f4ff)); // Google Blue
        return chosenColors;
    }

    // ReSharper disable InconsistentNaming
    /// <summary>
    /// Calculates the CIEDE2000 distance between two LAB color values.
    /// </summary>
    /// <param name="lab1">The first LAB color value.</param>
    /// <param name="lab2">The second LAB color value.</param>
    /// <returns>The CIEDE2000 distance between the two LAB color values.</returns>
    private static double CalculateCIEDE2000Distance(LAB lab1, LAB lab2)
    {
        double deltaLPrime = lab2.L - lab1.L;
        double meanL = (lab1.L + lab2.L) / 2;
        
        double cStar1 = Sqrt(Pow(lab1.A, 2) + Pow(lab1.B, 2));
        double cStar2 = Sqrt(Pow(lab2.A, 2) + Pow(lab2.B, 2));
        double meanC = (cStar1 + cStar2) / 2;

        double meanCPow7 = Pow(meanC, 7);
        const double twentyFivePow7 = 6_103_515_625; // 25^7

        double aPrime1 = lab1.A + lab1.A / 2 * (1 - Sqrt(meanCPow7 / (meanCPow7 + twentyFivePow7)));
        double aPrime2 = lab2.A + lab2.A / 2 * (1 - Sqrt(meanCPow7 / (meanCPow7 + twentyFivePow7)));
        
        double cPrime1 = Sqrt(Pow(aPrime1, 2) + Pow(aPrime1, 2));
        double cPrime2 = Sqrt(Pow(aPrime2, 2) + Pow(aPrime2, 2));
        double meanCPrime = (cPrime1 + cPrime2) / 2;
        double deltaCPrime = cPrime2 - cPrime1;

        double hPrime1 = Atan2(lab1.B, aPrime1) % 360;
        double hPrime2 = Atan2(lab2.B, aPrime2) % 360;

        double hPrimeDiff = Abs(hPrime1 - hPrime2);
        double delta_hPrime = hPrimeDiff switch
        {
            <= 180 => hPrime2 - hPrime1,
            > 180 when hPrime2 <= hPrime1 => hPrime2 - hPrime1 + 360,
            _ => hPrime2 - hPrime1 - 360
        };

        double deltaHPrime = 2 * Sqrt(cPrime1 * cPrime2) * Sin(delta_hPrime / 2);
        double meanHPrime = cPrime1 is 0 || cPrime2 is 0
            ? hPrime1 + hPrime2
            : hPrimeDiff switch
            {
                <= 180 => (hPrime2 + hPrime1) / 2,
                > 180 when hPrime2 + hPrime1 < 360 => (hPrime2 + hPrime1 + 360) / 2,
                _ => (hPrime2 + hPrime1 - 360) / 2
            };
        
        double t = 1 - 0.17 * Cos(meanHPrime - 30) + 0.24 * Cos(2 * meanHPrime) + 0.32 * Cos(3 * meanHPrime + 6) 
                   - 0.2 * Cos(4 * meanHPrime - 63);

        double sL = 1 + 0.015 * Pow(meanL - 50, 2) / Sqrt(20 + Pow(meanL - 50, 2));
        double sC = 1 + 0.045 * meanCPrime;
        double sH = 1 + 0.015 * meanCPrime * t;

        double rT = -2 * Sqrt(Pow(meanCPrime, 7) / (Pow(meanCPrime, 7) + twentyFivePow7)) *
                    Sin(60 * Exp(-Pow((meanHPrime - 275) / 25, 2)));

        double l = deltaLPrime / Pow(sL, 2);
        double c = deltaCPrime / Pow(sC, 2);
        double h = deltaHPrime / Pow(sH, 2);
        
        return Sqrt(Pow(l, 2) + Pow(c, 2) + Pow(h, 2) + rT * c * h);
    }
    // ReSharper restore InconsistentNaming
}