using MaterialDesign.Color.Contrast;
using static System.Math;
using _Contrast = MaterialDesign.Color.Contrast.Contrast;

namespace MaterialDesign.Color.Extensions;

public static class HCTAExtensions
{
    /// <summary>
    /// Generates a new HCTA color with the specified contrast ratio.
    /// </summary>
    /// <param name="current">The color to base the new color from.</param>
    /// <param name="source">The color to use the tone from.</param>
    /// <param name="ratio">The contrast ratio to generate the new color with.</param>
    /// <param name="darker">Whether the new color should be darker or lighter than the source.
    /// This will be automatically decided if the parameter is null or not provided.</param>
    /// <exception cref="InvalidOperationException">Thrown when <paramref name="darker"/> is null
    /// and neither a lighter nor darker tone can be created with the ratio, or when <paramref name="darker"/>
    /// is specified and the relevant tone cannot be created.</exception>
    public static HCTA ContrastTo(this HCTA current, HCTA source, double ratio, bool? darker = null)
    {
        double diff = Round(_Contrast.LighterViaRatio(0, ratio), 3, MidpointRounding.ToPositiveInfinity);

        if (darker is null)
        {
            double darkerTone = _Contrast.DarkerViaRatio(source.T, ratio);
            bool canBeDarker = darkerTone is not -1;
            double lighterTone = _Contrast.LighterViaRatio(source.T, ratio);
            bool canBeLighter = lighterTone is not -1;

            switch (canBeDarker)
            {
                case false when canBeLighter is false: // neither
                {
                    throw new InvalidOperationException($"Cannot create a color with a contrast of {ratio} from {source} " +
                                                        $"as it would require a minimum of {diff} in either tone direction.");
                }
                case true when canBeLighter: // both
                {
                    double darkFrom50 = Abs(50 - darkerTone);
                    double lightFrom50 = Abs(50 - lighterTone);

                    return new HCTA(current.H, current.C, lightFrom50 <= darkFrom50 ? lighterTone : darkerTone);
                }
                default:
                    return new HCTA(current.H, current.C, canBeDarker ? darkerTone : lighterTone);
            }
        }

        double tone = darker is true
            ? _Contrast.DarkerViaRatio(source.T, ratio)
            : _Contrast.LighterViaRatio(source.T, ratio);

        if (tone is -1)
        {
            double minTone = darker is true ? 100 - tone : tone;
            string minOrMax = darker is true ? "maximum" : "minimum";
            throw new InvalidOperationException($"Cannot create a color with a contrast of {ratio} from {source} " +
                                                $"as it would allow a ${minOrMax} source tone of {minTone}.");
        }

        return new HCTA(current.H, current.C, tone);
    }

    public static HCTA ContrastTo(this HCTA current, HCTA source, ContrastRatio ratio) =>
        current.ContrastTo(source, ratio.Ratio);
}