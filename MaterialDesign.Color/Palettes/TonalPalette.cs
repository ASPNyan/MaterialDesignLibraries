using System.Diagnostics.Contracts;
using MaterialDesign.Color.Colorspaces;

namespace MaterialDesign.Color.Palettes;

public readonly struct TonalPalette(HCTA hcta)
{
    public double Hue { get; } = hcta.H;
    public double Chroma { get; } = hcta.C;
    public HCTA KeyColor { get; } = hcta;

    public TonalPalette(Colorspaces.Color color) : this((HCTA)color)
    {
    }

    public TonalPalette(double hue, double chroma) : this(CreateKeyColor(hue, chroma))
    {
    }

    public TonalPalette(double hue, double chroma, HCTA keyColor) : this(keyColor)
    {
        Hue = hue;
        Chroma = chroma;
    }

    /// <summary>
    /// Gets the color of the current palette as a <see cref="HCTA"/> color with tone of <paramref name="tone"/>
    /// </summary>
    /// <param name="tone">The tone to get the color with.</param>
    /// <returns>The color of the current palette with the provided tone.</returns>
    [Pure]
    public HCTA GetWithTone(double tone) => new(Hue, Chroma, tone);

    /// <summary>
    /// Creates a new <see cref="HCTA"/> color with the specified hue and chroma values.
    /// </summary>
    /// <param name="hue">The hue value of the color.</param>
    /// <param name="chroma">The chroma value of the color.</param>
    /// <returns>The created HCTA color.</returns>
    private static HCTA CreateKeyColor(double hue, double chroma)
    {
        const double startTone = 50;

        HCTA smallestDeltaHCTA = new(hue, chroma, startTone);
        double smallestDelta = Math.Abs(smallestDeltaHCTA.C - chroma);

        for (double delta = 1; delta < 50; delta++)
        {
            if (Math.Abs(Math.Round(chroma) - Math.Round(smallestDeltaHCTA.C)) < 5e-5) return smallestDeltaHCTA;

            HCTA hctaAdd = new(hue, chroma, startTone + delta);
            double hctaAddDelta = Math.Abs(hctaAdd.C - chroma);

            if (hctaAddDelta < smallestDelta)
            {
                smallestDelta = hctaAddDelta;
                smallestDeltaHCTA = hctaAdd;
            }

            HCTA hctaSubtract = new(hue, chroma, startTone - delta);
            double hctaSubtractDelta = Math.Abs(hctaSubtract.C - chroma);

            if (hctaSubtractDelta < smallestDelta)
            {
                smallestDelta = hctaSubtractDelta;
                smallestDeltaHCTA = hctaSubtract;
            }
        }

        return smallestDeltaHCTA;
    }
}