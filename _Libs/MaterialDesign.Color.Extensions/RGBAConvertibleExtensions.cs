using MaterialDesign.Color.Quantize;

namespace MaterialDesign.Color.Extensions;

public static class RGBAConvertibleExtensions
{
    public static double DistanceFrom<TSelf, TFrom>(this IRGBAConvertible<TSelf> self, IRGBAConvertible<TFrom> from)
        where TSelf : IRGBAConvertible<TSelf> where TFrom : IRGBAConvertible<TFrom>
    {
        LAB labSelf = LAB.FromRGBA(self.ToRGBA());
        LAB labFrom = LAB.FromRGBA(from.ToRGBA());

        return LABPointProvider.Distance(labSelf, labFrom);
    }

    public static TSelf Invert<TSelf>(this IRGBAConvertible<TSelf> self) where TSelf : IRGBAConvertible<TSelf> =>
        TSelf.FromRGBA(~self.ToRGBA());

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