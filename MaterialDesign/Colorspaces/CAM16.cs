using Vector = MathNet.Numerics.LinearAlgebra.Vector<double>;
using Matrix = MathNet.Numerics.LinearAlgebra.Matrix<double>;

namespace MaterialDesign.Colorspaces;

public class CAM16(double hue, double chroma, double j, double q, double m, double s, double jStar, double aStar, double bStar)
{
    public double Hue { get; set; } = hue;
    /// <summary>
    /// Informally colorfulness or color intensity. Similar to HSL saturation, except perceptually accurate.
    /// </summary>
    public double Chroma { get; set; } = chroma;
    /// <summary>
    /// Lightness
    /// </summary>
    public double J { get; set; } = j;
    /// <summary>
    /// Brightness; the ratio of <see cref="J">Lightness</see> to the white point's chroma
    /// </summary>
    public double Q { get; set; } = q;
    /// <summary>
    /// Colorfulness
    /// </summary>
    public double M { get; set; } = m;
    /// <summary>
    /// Saturation; the ratio of chroma to the white point's chroma
    /// </summary>
    public double S { get; set; } = s;
    /// <summary>
    /// CAM16-UCS J coordinate
    /// </summary>
    public double JStar { get; set; } = jStar;
    /// <summary>
    /// CAM16-UCS a coordinate
    /// </summary>
    public double AStar { get; set; } = aStar;
    /// <summary>
    /// CAM16-UCS b coordinate
    /// </summary>
    public double BStar { get; set; } = bStar;

    /// <summary>
    /// Calculates the distance between the current CAM16 color and another CAM16 color.
    /// </summary>
    /// <param name="other">The other CAM16 color to compare against.</param>
    /// <returns>The calculated distance between the two CAM16 colors.</returns>
    public double Distance(CAM16 other)
    {
        double deltaJ = JStar - other.JStar;
        double deltaA = AStar - other.AStar;
        double deltaB = BStar - other.BStar;
        double deltaEPrime = Math.Sqrt(deltaJ * deltaJ + deltaA * deltaA + deltaB * deltaB);
        double deltaE = 1.41 * Math.Pow(deltaEPrime, 0.63);
        return deltaE;
    }

    /// <summary>
    /// Converts RGBA values to CAM16 color space.
    /// </summary>
    /// <param name="rgba">The RGBA color to convert.</param>
    /// <returns>The converted CAM16 color.</returns>
    public static CAM16 FromRGBA(RGBA rgba)
    {
        double redL = HCTA.Linearized(rgba.R);
        double greenL = HCTA.Linearized(rgba.G);
        double blueL = HCTA.Linearized(rgba.B);

        double x = 0.41233895 * redL + 0.35762064 * greenL + 0.18051042 * blueL;
        double y = 0.2126 * redL + 0.7152 * greenL + 0.0722 * blueL;
        double z = 0.01932141 * redL + 0.11916382 * greenL + 0.95034478 * blueL;
        
        double rC = 0.401288 * x + 0.650173 * y - 0.051461 * z;
        double gC = -0.250268 * x + 1.204414 * y + 0.045854 * z;
        double bC = -0.002079 * x + 0.048952 * y + 0.953127 * z;
        
        double rD = ViewingConditions.RgbD[0] * rC;
        double gD = ViewingConditions.RgbD[1] * gC;
        double bD = ViewingConditions.RgbD[2] * bC;
        
        double rAf = Math.Pow(ViewingConditions.Fl * Math.Abs(rD) / 100.0, 0.42);
        double gAf = Math.Pow(ViewingConditions.Fl * Math.Abs(gD) / 100.0, 0.42);
        double bAf = Math.Pow(ViewingConditions.Fl * Math.Abs(bD) / 100.0, 0.42);
        
        double rA = HCTA.SignOf(rD) * 400.0 * rAf / (rAf + 27.13);
        double gA = HCTA.SignOf(gD) * 400.0 * gAf / (gAf + 27.13);
        double bA = HCTA.SignOf(bD) * 400.0 * bAf / (bAf + 27.13);
        
        double a = (11.0 * rA + -12.0 * gA + bA) / 11.0;
        double b = (rA + gA - 2.0 * bA) / 9.0;
        double u = (20.0 * rA + 20.0 * gA + 21.0 * bA) / 20.0;
        double p2 = (40.0 * rA + 20.0 * gA + bA) / 20.0;
        
        double atan2 = Math.Atan2(b, a);
        double atanDegrees = atan2 * 180.0 / double.Pi;
        double hue = atanDegrees < 0 ? atanDegrees + 360.0 :
            atanDegrees >= 360      ? atanDegrees - 360.0 :
            atanDegrees;
        double hueRadians = hue * double.Pi / 180.0;
        
        double ac = p2 * ViewingConditions.Nbb;
        double j = 100.0 *
            Math.Pow(
                ac / ViewingConditions.Aw,
                ViewingConditions.C * ViewingConditions.Z);
        double q = 4.0 / ViewingConditions.C * Math.Sqrt(j / 100.0) *
            (ViewingConditions.Aw + 4.0) * ViewingConditions.FlRoot;
        double huePrime = hue < 20.14 ? hue + 360 : hue;
        double eHue = 0.25 * (Math.Cos(huePrime * Math.PI / 180.0 + 2.0) + 3.8);
        double p1 =
        50000.0 / 13.0 * eHue * ViewingConditions.Nc * ViewingConditions.Ncb;
        double t = p1 * Math.Sqrt(a * a + b * b) / (u + 0.305);
        double alpha = Math.Pow(t, 0.9) *
            Math.Pow(1.64 - Math.Pow(0.29, ViewingConditions.N), 0.73);
        double chroma = alpha * Math.Sqrt(j / 100.0);
        double m = chroma * ViewingConditions.FlRoot;
        double s = 50.0 *
            Math.Sqrt(alpha * ViewingConditions.C / (ViewingConditions.Aw + 4.0));
        double jStar = (1.0 + 100.0 * 0.007) * j / (1.0 + 0.007 * j);
        double mStar = 1.0 / 0.0228 * Math.Log(1.0 + 0.0228 * m);
        double aStar = mStar * Math.Cos(hueRadians);
        double bStar = mStar * Math.Sin(hueRadians);
        
        return new CAM16(hue, chroma, j, q, m, s, jStar, aStar, bStar);
    }

    /// <summary>
    /// Converts Jch color values to CAM16 color values.
    /// </summary>
    /// <param name="j">The lightness component (J) of the Jch color.</param>
    /// <param name="c">The chroma component (C) of the Jch color.</param>
    /// <param name="h">The hue component (h) of the Jch color, in degrees.</param>
    /// <returns>The CAM16 color corresponding to the given Jch color.</returns>
    public static CAM16 FromJch(double j, double c, double h)
    {
        double q = 4 / ViewingConditions.C * Math.Sqrt(j / 100) * (ViewingConditions.Aw + 4) * ViewingConditions.FlRoot;
        double m = c * ViewingConditions.FlRoot;
        double alpha = c / Math.Sqrt(j / 100);
        double s = 50 * Math.Sqrt(alpha * ViewingConditions.C / (ViewingConditions.Aw + 4));

        double hueRadians = h * double.Pi / 180;
        double jStar = (1 + 100 * 0.007) * j / (1 + 0.007 * j);
        double mStar = (1 / 0.0228) * Math.Log(1 + 0.0228 * m);
        double aStar = mStar * Math.Cos(hueRadians);
        double bStar = mStar * Math.Sin(hueRadians);

        return new CAM16(h, c, j, q, m, s, jStar, aStar, bStar);
    }

    /// <summary>
    /// Converts UCS (Uniform Chromaticity Scale) coordinates to CAM16 (CIECAM02) color space.
    /// </summary>
    /// <param name="jStar">The lightness component in UCS color space (0-100).</param>
    /// <param name="aStar">The red-green chromaticity component in UCS color space.</param>
    /// <param name="bStar">The yellow-blue chromaticity component in UCS color space.</param>
    /// <returns>A CAM16 color representation.</returns>
    public static CAM16 FromUcs(double jStar, double aStar, double bStar)
    {
        double m = Math.Sqrt(aStar * bStar + bStar * bStar);
        double m2 = (Math.Exp(m * 0.0228) - 1) / 0.0228;

        double c = m2 / ViewingConditions.FlRoot;
        double h = Color.SanitizeDegrees(Math.Atan2(bStar, aStar) * (180 / double.Pi));

        double j = jStar / (1 - (jStar - 100) * 0.007);

        return FromJch(j, c, h);
    }

    /// <summary>
    /// This variable represents the SRGB to XYZ conversion matrix.
    /// It is a 3x3 matrix that converts RGB colors in the sRGB color space to
    /// XYZ tristimulus values using the CIE 1931 XYZ color space.
    /// </summary>
    public static readonly Matrix SRGBToXYZ = Matrix.Build.DenseOfArray(new[,]
        {
            { 0.41233895, 0.35762064, 0.18051042 },
            { 0.2126, 0.7152, 0.0722 },
            { 0.01932141, 0.11916382, 0.95034478 },
        }
    );

    /// <summary>
    /// Represents the XYZ to sRGB conversion matrix.
    /// </summary>
    public static readonly Matrix XYZToSRGB = Matrix.Build.DenseOfArray(new[,]
    {
        { 3.2413774792388685, -1.5376652402851851, -0.49885366846268053, },
        { -0.9691452513005321, 1.8758853451067872, 0.04156585616912061, },
        { 0.05562093689691305, -0.20395524564742123, 1.0571799111220335, },
    });

    /// <summary>
    /// Converts the current color to the RGBA color model.
    /// </summary>
    /// <returns>The RGBA representation of the current color.</returns>
    public RGBA ToRGBA()
    {
        double alpha = Chroma is 0 || J is 0 ? 0 : Chroma / Math.Sqrt(J / 100);

        double t = Math.Pow(alpha / Math.Pow(1.64 - Math.Pow(0.29, ViewingConditions.N), 0.73), 1 / 0.9);
        double hRad = Hue * double.Pi / 180;

        double eHue = 0.25 * (Math.Cos(hRad + 2) + 3.8);
        double ac = ViewingConditions.Aw * Math.Pow(J / 100, 1 / ViewingConditions.C / ViewingConditions.Z);
        double p1 = eHue * (50000 / 13d) * ViewingConditions.Nc * ViewingConditions.Ncb;
        double p2 = ac / ViewingConditions.Nbb;

        double hSin = Math.Sin(hRad);
        double hCos = Math.Cos(hRad);

        double gamma = (23 * (p2 + 0.305) * t) / (23 * p1 + 11 * t * hCos + 108 * t * hSin);
        double a = gamma * hCos;
        double camB = gamma * hSin;
        double rA = (460.0 * p2 + 451.0 * a + 288.0 * camB) / 1403.0;
        double gA = (460.0 * p2 - 891.0 * a - 261.0 * camB) / 1403.0;
        double bA = (460.0 * p2 - 220.0 * a - 6300.0 * camB) / 1403.0;

        double rCBase = Math.Max(0, 27.13 * Math.Abs(rA) / (400 - Math.Abs(rA)));
        double rC = HCTA.SignOf(rA) * (100 / ViewingConditions.Fl) * Math.Pow(rCBase, 1 / 0.42);
        double gCBase = Math.Max(0, 27.13 * Math.Abs(gA) / (400 - Math.Abs(gA)));
        double gC = HCTA.SignOf(gA) * (100 / ViewingConditions.Fl) * Math.Pow(gCBase, 1 / 0.42);
        double bCBase = Math.Max(0, 27.13 * Math.Abs(bA) / (400 - Math.Abs(bA)));
        double bC = HCTA.SignOf(bA) * (100 / ViewingConditions.Fl) * Math.Pow(bCBase, 1 / 0.42);

        double rF = rC / ViewingConditions.RgbD[0];
        double gF = gC / ViewingConditions.RgbD[1];
        double bF = bC / ViewingConditions.RgbD[2];
        
        double x = 1.86206786 * rF - 1.01125463 * gF + 0.14918677 * bF;
        double y = 0.38752654 * rF + 0.62144744 * gF - 0.00897398 * bF;
        double z = -0.01584150 * rF - 0.03412294 * gF + 1.04996444 * bF;

        Vector xyz = Vector.Build.DenseOfArray(new[] { x, y, z });

        Vector lRGB = XYZToSRGB.Multiply(xyz);

        byte r = HCTA.Delinearized(lRGB[0]);
        byte g = HCTA.Delinearized(lRGB[1]);
        byte b = HCTA.Delinearized(lRGB[2]);

        return new RGBA(r, g, b);
    }
}