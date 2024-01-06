// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

using System.Diagnostics.Contracts;
using MaterialDesign.Color.Common;

namespace MaterialDesign.Color.Extensions;

// This was all done based on nburrus' CVD simulation project at https://github.com/DaltonLens/libDaltonLens
/// <summary>
/// A class containing static methods to modify colors using CVD (color vision deficiency) filtering.
/// </summary>
public static class ColorDeficiencyExtensions
{
    /// <summary>
    /// Simulates an RGBA color as a person with Protan- (red deficiency) Color Vision Deficiency
    /// with a specified severity using the Viénot et al. 1999 CVD simulation formula.
    /// </summary>
    /// <param name="source">The source color.</param>
    /// <param name="severity">The severity of the CVD, 0 (inclusive) to 1 (exclusive)</param>
    /// <returns>A new color with Protan CVD simulation.</returns>
    [Pure]
    public static RGBA SimulateProtan(this RGBA source, double severity)
    {
        Vector lRGB = Vector.From([source.R / 255d, source.G / 255d, source.B / 255d]);

        Vector rgbDeficiency = VienotProtan.RGBDeficiencyFromRGB.Multiply(lRGB);

        if (severity < 0.999)
        {
            for (int i = 0; i < 3; i++) rgbDeficiency[i] = rgbDeficiency[i] * severity + lRGB[i] * (1 - severity);
        }

        byte r = (byte)Colorspaces.Color.Round0(rgbDeficiency[0] * 255);
        byte g = (byte)Colorspaces.Color.Round0(rgbDeficiency[1] * 255);
        byte b = (byte)Colorspaces.Color.Round0(rgbDeficiency[2] * 255);

        return new RGBA(r, g, b, source.A);
    }
    
    /// <summary>
    /// Simulates an RGBA color as a person with Deutan- (green deficiency) Color Vision Deficiency
    /// with a specified severity using the Viénot et al. 1999 CVD simulation formula.
    /// </summary>
    /// <param name="source">The source color.</param>
    /// <param name="severity">The severity of the CVD, 0 (inclusive) to 1 (exclusive)</param>
    /// <returns>A new color with Deutan CVD simulation.</returns>
    public static RGBA SimulateDeutan(this RGBA source, double severity)
    {
        Vector lRGB = Vector.From([source.R / 255d, source.G / 255d, source.B / 255d]);

        Vector rgbDeficiency = VienotDeutan.RGBDeficiencyFromRGB.Multiply(lRGB);

        if (severity < 0.999)
        {
            for (int i = 0; i < 3; i++) rgbDeficiency[i] = rgbDeficiency[i] * severity + lRGB[i] * (1 - severity);
        }

        byte r = (byte)Colorspaces.Color.Round0(rgbDeficiency[0] * 255);
        byte g = (byte)Colorspaces.Color.Round0(rgbDeficiency[1] * 255);
        byte b = (byte)Colorspaces.Color.Round0(rgbDeficiency[2] * 255);

        return new RGBA(r, g, b, source.A);
    }
    
    /// <summary>
    /// Simulates an RGBA color as a person with Tritan- (blue deficiency) Color Vision Deficiency
    /// with a specified severity using the Brettel et al. 1997 CVD simulation formula.
    /// </summary>
    /// <param name="source">The source color.</param>
    /// <param name="severity">The severity of the CVD, 0 (inclusive) to 1 (exclusive)</param>
    /// <returns>A new color with Tritan CVD simulation.</returns>
    public static RGBA SimulateTritan(this RGBA source, double severity)
    {
        Vector lRGB = Vector.From([source.R / 255d, source.G / 255d, source.B / 255d]);

        double dotWithSepPlane = lRGB.DotProduct(BrettelTritan.SeparationPlaneNormalInRGB);
        Matrix rgbDeficiencyFromRGB = dotWithSepPlane >= 0
            ? BrettelTritan.RGBDeficiencyFromRGB1
            : BrettelTritan.RGBDeficiencyFromRGB2;

        Vector rgbDeficiency = rgbDeficiencyFromRGB.Multiply(lRGB);

        for (int i = 0; i < 3; i++) rgbDeficiency[i] = rgbDeficiency[i] * severity + lRGB[i] * (1 - severity);

        byte r = (byte)Colorspaces.Color.Round0(rgbDeficiency[0] * 255);
        byte g = (byte)Colorspaces.Color.Round0(rgbDeficiency[1] * 255);
        byte b = (byte)Colorspaces.Color.Round0(rgbDeficiency[2] * 255);

        return new RGBA(r, g, b, source.A);
    }

    private static Vienot1999 VienotProtan { get; } = new()
    {
        RGBDeficiencyFromRGB = Matrix.From([[0.11238, 0.88762, 0.00000],
                                            [0.11238, 0.88762, -0.00000],
                                            [0.00401, -0.00401, 1.00000]])
    };
    
    private static Vienot1999 VienotDeutan { get; } = new()
    {
        RGBDeficiencyFromRGB = Matrix.From([[0.29275, 0.70725, 0.00000],
                                            [0.29275, 0.70725, -0.00000],
                                            [-0.02234, 0.02234, 1.00000]])
    };
    
    private static Brettel1997 BrettelTritan { get; } = new()
    {
        RGBDeficiencyFromRGB1 = Matrix.From([[1.01277, 0.13548, -0.14826],
                                             [-0.01243, 0.86812, 0.14431],
                                             [0.07589, 0.80500, 0.11911]]),
        
        RGBDeficiencyFromRGB2 = Matrix.From([[0.93678, 0.18979, -0.12657],
                                             [0.06154, 0.81526, 0.12320],
                                             [-0.37562, 1.12767, 0.24796]]),
        
        SeparationPlaneNormalInRGB = Vector.From([0.03901, -0.02788, -0.01113])
    };
    
    private readonly struct Brettel1997
    {
        public required Matrix RGBDeficiencyFromRGB1 { get; init; } // Transformation using Plane 1.
        public required Matrix RGBDeficiencyFromRGB2 { get; init; } // Transformation using Plane 2.
        public required Vector SeparationPlaneNormalInRGB { get; init; } // Normal of the separation plane to pick the right transform, already in RGB.
    }

    // yes it's actually Viénot, but you're a fool if you think I'm using letter accents in code.
    private readonly struct Vienot1999
    {
        public required Matrix RGBDeficiencyFromRGB { get; init; }
    }
}