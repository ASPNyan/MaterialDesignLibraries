using System.Diagnostics.Contracts;
using MaterialDesign.Color.Quantize;

namespace MaterialDesign.Color.Extensions;

public static class RGBAConvertibleExtensions
{
    /// <summary>
    /// Calculates the distance between two colors using the Euclidean distance formula by conversion to <see cref="LAB"/>.
    /// </summary>
    /// <returns>The distance between the two colors.</returns>
    [Pure]
    public static double DistanceFrom<TSelf, TFrom>(this IRGBAConvertible<TSelf> self, IRGBAConvertible<TFrom> from)
        where TSelf : IRGBAConvertible<TSelf> where TFrom : IRGBAConvertible<TFrom>
    {
        LAB labSelf = LAB.FromRGBA(self.ToRGBA());
        LAB labFrom = LAB.FromRGBA(from.ToRGBA());

        return LABPointProvider.Distance(labSelf, labFrom);
    }

    /// <summary>
    /// Inverts a color by converting to <see cref="RGBA"/>, inverting the non-alpha channels, and converting it back.
    /// Does not modify the original.
    /// </summary>
    /// <returns>The color, inverted in the RGB colorspace.</returns>
    [Pure]
    public static TSelf Invert<TSelf>(this IRGBAConvertible<TSelf> self) where TSelf : IRGBAConvertible<TSelf> =>
        TSelf.FromRGBA(~self.ToRGBA());

    /// <summary>
    /// Creates a new color based on the provided one with a specified severity of a CVD.
    /// </summary>
    [Pure]
    public static TSelf SimulateColorDeficiency<TSelf>(this IRGBAConvertible<TSelf> self, float severity,
        ColorDeficiency deficiency) where TSelf : IRGBAConvertible<TSelf>
    {
        return TSelf.FromRGBA(deficiency switch
        {
            ColorDeficiency.Protan => self.ToRGBA().SimulateProtan(severity),
            ColorDeficiency.Deutan => self.ToRGBA().SimulateDeutan(severity),
            ColorDeficiency.Tritan => self.ToRGBA().SimulateTritan(severity),
            _ => throw new ArgumentOutOfRangeException(nameof(deficiency), deficiency, null)
        });
    }
}