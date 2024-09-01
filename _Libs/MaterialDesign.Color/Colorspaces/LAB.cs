using System.Numerics;

namespace MaterialDesign.Color.Colorspaces;

/// <summary>
/// Represents a color in LAB color space.
/// </summary>
public class LAB(double l, double a, double b) : IRGBAConvertible<LAB>
{
    /// <summary>
    /// Gets or sets the lightness value of the color. 
    /// The value ranges from 0 to 100, where 0 represents perfect black and 100 represents perfect white.
    /// </summary>
    public double L { get; set; } = l;
    /// <summary>
    /// Gets or sets the 'A' value of the color. This represents color variance from green to red.
    /// Negative values indicate green while positive values indicate red.
    /// </summary>
    public double A { get; set; } = a;
    /// <summary>
    /// Gets or sets the 'B' value of the color. This represents color variance from blue to yellow.
    /// Negative values indicate blue while positive values indicate yellow.
    /// </summary>
    public double B { get; set; } = b;
    
    private static readonly double[] WhitePointD65 = {95.047, 100.0, 108.883};

    /// <summary>
    /// Calculates the DeltaE value between the current LAB color and the specified LAB color.
    /// </summary>
    /// <param name="lab">The LAB color to compare with.</param>
    /// <returns>The DeltaE value between the two LAB colors.</returns>
    public double DeltaE(LAB lab)
    {
        double dL = L - lab.L;
        double dA = A - lab.A;
        double dB = B - lab.B;
        return dL * dL + dA * dA + dB * dB;
    }

    private const double E = 216.0 / 24389.0;
    private const double Kappa = 24389.0 / 27.0;
    private const double Ke = 8;

    /// <summary>
    /// Converts the current LAB color to the RGBA color model.
    /// </summary>
    /// <returns>The RGBA representation of the current color.</returns>
    public RGBA ToRGBA()
    {
        double fy = (L + 16.0) / 116.0;
        double fx = A / 500.0 + fy;
        double fz = fy - B / 200.0;
        double fx3 = fx * fx * fx;
        double xNormalized = fx3 > E ? fx3 : (116.0 * fx - 16.0) / Kappa;
        double yNormalized = L > Ke ? fy * fy * fy : L / Kappa;
        double fz3 = fz * fz * fz;
        double zNormalized = fz3 > E ? fz3 : (116.0 * fz - 16.0) / Kappa;
        
        double x = xNormalized * WhitePointD65[0];
        double y = yNormalized * WhitePointD65[1];
        double z = zNormalized * WhitePointD65[2];
        
        double rL = 3.2406 * x - 1.5372 * y - 0.4986 * z;
        double gL = -0.9689 * x + 1.8758 * y + 0.0415 * z;
        double bL = 0.0557 * x - 0.2040 * y + 1.0570 * z;

        (byte r, byte g, byte b) = HCTA.Delinearized(new Vector3((float)rL, (float)gL, (float)bL));

        return new RGBA(r, g, b);
    }

    /// <summary>
    /// Creates a CIE LAB color from an existing RGBA color.
    /// </summary>
    /// <returns>The LAB representation of an RGBA color</returns>
    public static LAB FromRGBA(RGBA rep)
    {
        byte red = rep.R;
        byte green = rep.G;
        byte blue = rep.B;
        double redL = HCTA.Linearized(red);
        double greenL = HCTA.Linearized(green);
        double blueL = HCTA.Linearized(blue);
        double x = 0.41233895 * redL + 0.35762064 * greenL + 0.18051042 * blueL;
        double y = 0.2126 * redL + 0.7152 * greenL + 0.0722 * blueL;
        double z = 0.01932141 * redL + 0.11916382 * greenL + 0.95034478 * blueL;
        double yNormalized = y / WhitePointD65[1];
        double fy;

        if (yNormalized > E) 
        {
            fy = Math.Pow(yNormalized, 1.0 / 3.0);
        } else 
        {
            fy = (Kappa * yNormalized + 16) / 116;
        }

        double xNormalized = x / WhitePointD65[0];
        double fx = xNormalized > E ? Math.Pow(xNormalized, 1.0 / 3.0) : (Kappa * xNormalized + 16) / 116;

        double zNormalized = z / WhitePointD65[2];
        double fz = zNormalized > E ? Math.Pow(zNormalized, 1.0 / 3.0) : (Kappa * zNormalized + 16) / 116;

        double l = 116.0 * fy - 16;
        double a = 500.0 * (fx - fy);
        double b = 200.0 * (fy - fz);
        return new LAB(l, a, b);
    }
}