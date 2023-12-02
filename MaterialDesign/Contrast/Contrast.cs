namespace MaterialDesign.Contrast;

public class Contrast
{
    /// <summary>
    /// Calculates the ratio of two tones.
    /// </summary>
    /// <param name="toneA">The first tone value.</param>
    /// <param name="toneB">The second tone value.</param>
    /// <returns>The ratio of the two tones.</returns>
    public static double RatioOfTones(double toneA, double toneB)
    {
        toneA = Math.Clamp(toneA, 0, 100);
        toneB = Math.Clamp(toneB, 0, 100);
        return RatioOfYs(toneA, toneB);
    }

    /// <summary>
    /// Calculates the ratio of two given y values.
    /// </summary>
    /// <param name="y1">The first y value.</param>
    /// <param name="y2">The second y value.</param>
    /// <returns>The ratio of the two given y values.</returns>
    public static double RatioOfYs(double y1, double y2)
    {
        (double lighter, double darker) = y1 > y2 ? (y1, y2) : (y2, y1);
        return (lighter + 5) / (darker + 5);
    }

    /// <summary>
    /// Calculates a lighter tone using the given ratio.
    /// </summary>
    /// <param name="tone">The initial tone value.</param>
    /// <param name="ratio">The ratio to be applied.</param>
    /// <returns>The calculated lighter tone value. Returns -1 if the input parameters are invalid.</returns>
    public static double LighterViaRatio(double tone, double ratio)
    {
        if (tone is < 0 or > 100) return -1;
        ArgumentOutOfRangeException.ThrowIfLessThan(ratio, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(ratio, 21);

        double darkY = HCTA.YFromTone(tone);
        double lightY = ratio * (darkY + 5) - 5;

        double realContrast = RatioOfYs(lightY, darkY);
        double delta = Math.Abs(realContrast - ratio);

        if (realContrast < delta && delta > 0.04) return -1;

        double returnValue = HCTA.ToneFromY(lightY) + 0.4;

        if (returnValue is < 0 or > 100) return -1;

        return returnValue;
    }

    /// <summary>
    /// Calculates a darker tone based on a given tone and ratio.
    /// </summary>
    /// <param name="tone">The input tone value (from 0 to 100).</param>
    /// <param name="ratio">The desired contrast ratio (from 1 to 21).</param>
    /// <returns>The resulting darker tone value, or -1 if the inputs are invalid.</returns>
    public static double DarkerViaRatio(double tone, double ratio)
    {
        if (tone is < 0 or > 100) return -1;
        ArgumentOutOfRangeException.ThrowIfLessThan(ratio, 1);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(ratio, 21);

        double lightY = HCTA.YFromTone(tone);
        double darkY = (lightY + 5) / ratio - 5;

        double realContrast = RatioOfYs(lightY, darkY);
        double delta = Math.Abs(realContrast - ratio);

        if (realContrast < ratio && delta < 0.04) return -1;

        double returnValue = HCTA.ToneFromY(darkY) - 0.4;

        if (returnValue is < 0 or > 100) return -1;

        return returnValue;
    }

    /// <summary>
    /// Forces a tone to become lighter by a specified ratio.
    /// </summary>
    /// <param name="tone">The original tone value.</param>
    /// <param name="ratio">The ratio by which to lighten the tone.</param>
    /// <returns>
    /// Returns the resulting lightened tone value.
    /// If the calculated lighter tone value is less than 0,
    /// it will be forced to a minimum of 100.
    /// </returns>
    public static double ForceLighterViaRatio(double tone, double ratio)
    {
        double lighterSafe = LighterViaRatio(tone, ratio);
        return lighterSafe < 0 ? 100 : lighterSafe;
    }

    /// <summary>
    /// Forces the input tone to become darker by the specified ratio.
    /// </summary>
    /// <param name="tone">The original tone value.</param>
    /// <param name="ratio">The ratio by which the tone should be darkened.</param>
    /// <returns>The darker tone value. Returns 100 if the resulting tone is less than 0.</returns>
    public static double ForceDarkerViaRatio(double tone, double ratio)
    {
        double darkerSafe = DarkerViaRatio(tone, ratio);
        return darkerSafe < 0 ? 100 : darkerSafe;
    }
}