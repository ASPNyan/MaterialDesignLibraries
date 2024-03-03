using System.Diagnostics.Contracts;
using static System.Math;

namespace MaterialDesign.Color.Colorspaces;

/// <summary>
/// HCTA is a color container based on the Hue-Chroma-Tone-Alpha model. This model provides a more
/// perceptually meaningful way of representing color in some contexts compared to the traditional RGB model.
/// Alpha is also supported and is represented as a float ranging from 0-100. To access a byte implementation of
/// the alpha, there is the get-only <see cref="A255"/> property.
/// </summary>
/// <remarks>
/// The HCTA color space can theoretically represent an infinite number of colors. However, due to limitations of
/// floating point storage, this class can only store so many different colors. While this number is high, it is
/// also limited to the technically displayable colors, which is a total of <c>2^8 * 2^8 * 2^8</c> (or 16,777,216)
/// different colors, the practical limit for digital screens. It's important to note that while HCTA provides a
/// different way of thinking about color, the total number of unique colors it can represent is still bound by these
/// digital constraints while it's used on a display.
/// </remarks>
public class HCTA(double h, double c, double t, float a = 100) : IAlpha, IEquatable<IRGB>, 
    IEquatable<Color>, IEquatable<HCTA>, IRGBAConvertible<HCTA>
{
    private const double Precision = 5e-5;
    
    private readonly Guid _hashCode = Guid.NewGuid();
    
    /// <summary>
    /// The Hue value of the HCT color, ranging from 0 to 359 where 360 wraps around to 0.
    /// Values outside the range will be corrected to the equivalent 0 to 359 value.
    /// </summary>
    public double H
    {
        get => h;
        set => h = Color.SanitizeDegrees(value);
    }

    /// <summary>
    /// The Chroma value of the HCT color, ranging from 0 to 150 where values outside these bounds are clamped.
    /// </summary>
    public double C
    {
        get => c;
        set => c = Clamp(value, 0, 150);
    }

    /// <summary>
    /// The Tone value of the HCT color, ranging from 0 to 100 where values outside these bounds are clamped.
    /// </summary>
    public double T // T may also be known as L*, but it will be referred to as T or Tone in HCT
    {
        get => t;
        set => t = Clamp(value, 0, 100);
    }
    
    public float A
    {
        get => a;
        set => a = Clamp(value, 0, 100);
    }

    public byte A255 => (this as IAlpha).A255;

    byte IAlpha.A255 => byte.Clamp((byte)Color.Round0(A / 100 * 255), 0, 255);


    #region Conversion Helpers

    /// <summary>
    /// Converts a gamma-encoded RGB component to its linearized value.
    /// </summary>
    /// <param name="rgbComp">The gamma-encoded RGB component value.</param>
    /// <returns>The corresponding linearized value of the RGB component.</returns>
    public static double Linearized(byte rgbComp)
    {
        double normalized = rgbComp / 255d;
        if (normalized <= 0.0404499936) return normalized / 12.92 * 100;
        return Pow((normalized + 0.055) / 1.055, 2.4) * 100;
    }

    /// <summary>
    /// Delinearizes the given RGB component.
    /// </summary>
    /// <param name="rgbComp">The RGB component to delinearize.</param>
    /// <returns>The delinearized RGB component.</returns>
    public static byte Delinearized(double rgbComp)
    {
        double normalized = rgbComp / 100;

        double delinearized = normalized <= 0.0031308
            ? normalized * 12.92
            : 1.05 * Pow(normalized, 1.0 / 2.4) - 0.055;

        double rounded = Color.Round0(delinearized * 255);
        return Clamp((byte)rounded, byte.MinValue, byte.MaxValue);
    }

    /// <summary>
    /// Determines the sign of a given value.
    /// </summary>
    /// <param name="val">The value for which the sign needs to be determined.</param>
    /// <returns>
    /// -1 if the value is negative,
    /// 0 if the value is zero,
    /// 1 if the value is positive.
    /// </returns>
    public static int SignOf(double val)
    {
        return val switch
        {
            < 0 => -1,
            0 => 0,
            _ => 1
        };
    }

    /// <summary>
    /// Performs chromatic adaptation on the given component.
    /// </summary>
    /// <param name="component">The component to perform chromatic adaptation on.</param>
    /// <returns>The chromatically adapted value of the component.</returns>
    private static double ChromaticAdaptation(double component)
    {
        double af = Pow(Abs(component), 0.42);

        int sign = SignOf(component);

        return sign * 400 * af / (af + 27.13);
    }

    /// <summary>
    /// Applies inverse chromatic adaptation to the input value.
    /// </summary>
    /// <param name="adapted">The adapted value to be transformed.</param>
    /// <returns>The transformed value after applying inverse chromatic adaptation.</returns>
    private static double InverseChromaticAdaptation(double adapted)
    {
        double abs = Abs(adapted);
        double b = Max(0, 27.13 * abs / (400 - abs));

        int sign = SignOf(adapted);

        return sign * Pow(b, 1 / 0.42);
    }

    /// <summary>
    /// Converts linear RGB values to RGBA color format.
    /// </summary>
    /// <param name="linRGB">The linear RGB values as a Vector.</param>
    /// <returns>The resulting RGBA color.</returns>
    private static RGBA RGBAFromLinRGB(Vector linRGB)
    {
        byte r = Delinearized(linRGB[0]);
        byte g = Delinearized(linRGB[1]);
        byte b = Delinearized(linRGB[2]);

        return new RGBA(r, g, b);
    }

    /// <summary>
    /// Determines whether the given value is bounded between 0.0 and 100.0 (inclusive).
    /// </summary>
    /// <param name="x">The value to check for boundedness.</param>
    /// <returns>Returns true if the value is bounded between 0.0 and 100.0 (inclusive), false otherwise.</returns>
    private static bool IsBounded(double x)
    {
        return x is >= 0.0 and <= 100.0;
    }

    /// <summary>
    /// Calculates the Nth vertex based on the given y-coordinate and vertex index.
    /// </summary>
    /// <param name="y">The y-coordinate.</param>
    /// <param name="n">The index of the vertex.</param>
    /// <returns>The calculated Nth vertex as a Vector.</returns>
    private static Vector NthVertex(double y, int n)
    {
        double kR = YFromLinRGB[0];
        double kG = YFromLinRGB[1];
        double kB = YFromLinRGB[2];

        double coordA = n % 4 <= 1 ? 0 : 100;
        double coordB = n % 2 is 0 ? 0 : 100;

        switch (n)
        {
            case < 4:
            {
                double r = (y - coordA * kG - coordB * kB) / kR;

                if (IsBounded(r)) return Vector.From(new[] { r, coordA, coordB });
                return Vector.From(new double[] { -1, -1, -1 });
            }
            case < 8:
            {
                double g = (y - coordB * kR - coordA * kB) / kG;

                if (IsBounded(g)) return Vector.From(new[] { coordB, g, coordA });
                return Vector.From(new double[] { -1, -1, -1 });
            }
            default:
            {
                double b = (y - coordA * kR - coordB * kG) / kB;

                if (IsBounded(b)) return Vector.From(new[] { coordA, coordB, b });
                return Vector.From(new double[] { -1, -1, -1 });
            }
        }
    }

    /// <summary>
    /// Calculate the hue value of a given linear RGB color.
    /// </summary>
    /// <param name="linRGB">The linear RGB color.</param>
    /// <returns>The hue value.</returns>
    private static double HueOf(in Vector linRGB)
    {
        Vector scaledDiscount = ScaledDiscountFromLinRGB.Multiply(linRGB);

        double rA = ChromaticAdaptation(scaledDiscount[0]);
        double gA = ChromaticAdaptation(scaledDiscount[1]);
        double bA = ChromaticAdaptation(scaledDiscount[2]);

        double a = (11 * rA + -12 * gA + bA) / 11;
        double b = (rA + gA - 2 * bA) / 9;
        
        return Atan2(b, a);
    }

    /// <summary>
    /// Determines if three given angles are in a cyclic order, where values are in radians.
    /// </summary>
    /// <param name="a">The value of the first angle.</param>
    /// <param name="b">The value of the second angle.</param>
    /// <param name="c">The value of the third angle.</param>
    /// <returns>
    /// True if the angles are in a cyclic order, otherwise false.
    /// </returns>
    private static bool AreInCyclicOrder(double a, double b, double c)
    {
        double deltaAb = SanitizeRadians(b - a);
        double deltaAc = SanitizeRadians(c - a);
        return deltaAb < deltaAc;
    }

    /// <summary>
    /// Sanitizes an angle given in radians to be within the range of 0 to 2*pi.
    /// </summary>
    /// <param name="angle">The angle in radians to sanitize.</param>
    /// <returns>The sanitized angle, between 0 and 2*pi.</returns>
    private static double SanitizeRadians(double angle) => (angle + double.Pi * 8) % (double.Pi * 2);

    /// <summary>
    /// Bisects a segment on a given y-axis value and target hue.
    /// </summary>
    /// <param name="y">The y-axis value.</param>
    /// <param name="targetHue">The target hue.</param>
    /// <param name="val">An array containing the left and right vectors of the bisected segment.</param>
    private static void BisectToSegment(double y, double targetHue, out Vector[] val)
    {
        Vector left = Vector.From(new double[] { -1, -1, -1 });
        Vector right = left;

        double leftHue = 0;
        double rightHue = 0;

        bool initialized = false;
        bool uncut = true;

        for (int n = 0; n < 12; n++)
        {
            Vector mid = NthVertex(y, n);
            if (mid[0] < 0) continue;

            double midHue = HueOf(in mid);

            if (!initialized)
            {
                left = mid;
                right = mid;
                leftHue = midHue;
                rightHue = midHue;
                initialized = true;
                continue;
            }

            if (uncut || AreInCyclicOrder(leftHue, midHue, rightHue))
            {
                uncut = false;

                if (AreInCyclicOrder(leftHue, targetHue, midHue))
                {
                    right = mid;
                    rightHue = midHue;
                }
                else
                {
                    left = mid;
                    leftHue = midHue;
                }
            }
        }
        
        val = new[] { left, right };
    }

    /// <summary>
    /// Returns the value of the specified axis from the given Vector object.
    /// </summary>
    /// <param name="vector">The Vector object from which to retrieve the axis value.</param>
    /// <param name="axis">The index of the axis to retrieve the value from.</param>
    /// <returns>The value of the specified axis from the Vector object. Returns -1 if the axis index is out of bounds.</returns>
    private static double GetAxis(Vector vector, int axis)
    {
        return axis switch
        {
            0 => vector[0],
            1 => vector[1],
            2 => vector[2],
            _ => -1
        };
    }

    /// <summary>
    /// Returns the integer value that is the floor of the given double value minus 0.5.
    /// </summary>
    /// <param name="x">The double value to be evaluated</param>
    /// <returns>The floor value of x minus 0.5 as an integer</returns>
    private static int CriticalPlaneBelow(double x)
    {
        return (int)Floor(x - 0.5);
    }

    /// <summary>
    /// Calculates the critical plane value above the given input value.
    /// </summary>
    /// <param name="x">The input value.</param>
    /// <returns>The critical plane value above the input value.</returns>
    private static int CriticalPlaneAbove(double x)
    {
        return (int)Ceiling(x - 0.5);
    }

    /// <summary>
    /// Linearizes the given component value based on the sRGB color space formula.
    /// </summary>
    /// <param name="component">The component value to be delinearized.</param>
    /// <returns>The delinearized component value, scaled to the range of 0 to 255.</returns>
    private static double TrueDelinearize(double component)
    {
        double normalized = component / 100;

        double delinearized = normalized <= 0.0031308 ? normalized * 12.92 : 1.055 * Pow(normalized, 1 / 2.4) - 0.055;

        return delinearized * 255;
    }

    /// <summary>
    /// Calculates the intercept value between three given points: source
    /// , mid, and target.
    /// </summary>
    /// <param name="source">The value of the source point.</param>
    /// <param name="mid">The value of the mid point.</param>
    /// <param name="target">The value of the target point.</param>
    /// <returns>The intercept value between the source and target points along the mid point.</returns>
    private static double Intercept(double source, double mid, double target) => (mid - source) / (target - source);

    /// <summary>
    /// Linearly interpolates between two points in a three-dimensional vector space.
    /// </summary>
    /// <param name="source">The starting point of the interpolation.</param>
    /// <param name="t">The interpolation factor (between 0 and 1).</param>
    /// <param name="target">The ending point of the interpolation.</param>
    /// <returns>The interpolated point between the source and target points.</returns>
    private static Vector LerpPoint(Vector source, double t, Vector target) =>
        Vector.From(new[]
        {
            source[0] + (target[0] - source[0]) * t,
            source[1] + (target[1] - source[1]) * t,
            source[2] + (target[2] - source[2]) * t
        });

    /// <summary>
    /// Sets the specified coordinate of the target vector using linear interpolation
    /// . </summary> <param name="source">The source vector.</param> <param name="coordinate">The coordinate value to set.</param> <param name="target">The target vector.</param>
    /// <param name="axis">The axis along which to interpolate the target coordinate.</param> <returns>The target vector with the specified coordinate set.</returns>
    /// /
    private static Vector SetCoordinate(Vector source, double coordinate, Vector target, int axis)
    {
        double t = Intercept(GetAxis(source, axis), coordinate, GetAxis(target, axis));
        return LerpPoint(source, t, target);
    }

    /// <summary>
    /// Calculates the midpoint between two Vector objects.
    /// </summary>
    /// <param name="a">The first Vector.</param>
    /// <param name="b">The second Vector.</param>
    /// <returns>The midpoint between the two input Vector objects.</returns>
    private static Vector Midpoint(Vector a, Vector b) =>
        Vector.From(new[]
        {
            (a[0] + b[0]) / 2,
            (a[1] + b[1]) / 2,
            (a[2] + b[2]) / 2
        });

    /// <summary>
    /// Bisection method to find a vector that satisfies a certain condition.
    /// </summary>
    /// <param name="y">The y-coordinate of the target vector.</param>
    /// <param name="targetHue">The target hue value.</param>
    /// <returns>The vector that satisfies the condition.</returns>
    private static Vector BisectToLimit(double y, double targetHue)
    {
        BisectToSegment(y, targetHue, out Vector[] segment);

        Vector left = segment[0];
        double leftHue = HueOf(in left);
        Vector right = segment[1];

        for (int axis = 0; axis < 3; axis++)
        {
            if (Abs(GetAxis(left, axis) - GetAxis(right, axis)) > Precision)
            {
                int lPlane;
                int rPlane;

                if (GetAxis(left, axis) < GetAxis(right, axis))
                {
                    lPlane = CriticalPlaneBelow(TrueDelinearize(GetAxis(left, axis)));
                    rPlane = CriticalPlaneAbove(TrueDelinearize(GetAxis(right, axis)));
                }
                else
                {
                    lPlane = CriticalPlaneAbove(TrueDelinearize(GetAxis(left, axis)));
                    rPlane = CriticalPlaneBelow(TrueDelinearize(GetAxis(right, axis)));
                }
                
                for (int i = 0; i < 8; i++)
                {
                    if (Abs(rPlane - lPlane) <= 1) break;

                    int mPlane = (int)Floor((lPlane + rPlane) / 2d);
                    double midPlaneCoord = CriticalPlanes[mPlane];

                    Vector mid = SetCoordinate(left, midPlaneCoord, right, axis);

                    double midHue = HueOf(in mid);

                    if (AreInCyclicOrder(leftHue, targetHue, midHue))
                    {
                        right = mid;
                        rPlane = mPlane;
                    }
                    else
                    {
                        left = mid;
                        leftHue = midHue;
                        lPlane = mPlane;
                    }
                }
            }
        }

        return Midpoint(left, right);
    }

    /// <summary>
    /// Calculates the tone value from the given Y value.
    /// </summary>
    /// <param name="y">The Y value to calculate the tone from. Should be between 0 and 100.</param>
    /// <returns>The calculated tone value.</returns>
    public static double ToneFromY(double y)
    {
        const double e = 216 / 24389d;
        double yNormalized = y / 100;
        if (yNormalized <= e) return 24389 / 27d * yNormalized;
        return 116 * Pow(yNormalized, 1 / 3d) - 16;
    }

    /// <summary>
    /// Calculates the tone value from the RGBA color values.
    /// </summary>
    /// <param name="rgba">The RGBA color values.</param>
    /// <returns>The tone value calculated from the RGBA color values.</returns>
    private static double ToneFromRGBA(RGBA rgba)
    {
        double redL = Linearized(rgba.R);
        double greenL = Linearized(rgba.G);
        double blueL = Linearized(rgba.B);

        double y = 0.2126 * redL + 0.7152 * greenL + 0.0722 * blueL;
        return ToneFromY(y);
    }

    /// <summary>
    /// Converts a value from the CIE L*ab color space to the lightness value Y in the CIE XYZ color space.
    /// </summary>
    /// <param name="t">The value to convert.</param>
    /// <returns>The lightness value Y in the CIE XYZ color space.</returns>
    public static double YFromTone(double t)
    {
        const double ke = 8;
        if (t > ke)
        {
            double cbrt = (t + 16) / 116;
            double cube = cbrt * cbrt * cbrt;
            return cube * 100;
        }

        return t / (24389 / 27d) * 100;
    }

    #endregion
    
    #region To RGB

    /// <summary>
    /// Calculates the Y value from the current tone value.
    /// </summary>
    /// <returns>The Y value calculated from the current tone value.</returns>
    [Pure]
    private double YFromTone() => YFromTone(T);

    /// <summary>
    /// Creates an RGBA color based on the current tone value.
    /// </summary>
    /// <returns>Returns an RGBA color.</returns>
    [Pure]
    private RGBA RGBAFromTone()
    {
        double y = YFromTone();
        byte component = Delinearized(y);
        return new RGBA(component, component, component, A);
    }

    /// <summary>
    /// Matrix used to convert scaled discount to Linear RGB values.
    /// </summary>
    private static readonly Matrix LinRGBFromScaledDiscount = Matrix.From(
    [
        [
            1373.2198709594231,
            -1100.4251190754821,
            -7.278681089101213,
        ],
        [
            -271.815969077903,
            559.6580465940733,
            -32.46047482791194,
        ],
        [
            1.9622899599665666,
            -57.173814538844006,
            308.7233197812385,
        ]
    ]);

    /// <summary>
    /// Represents a matrix containing scaled discount values based on linear RGB values.
    /// </summary>
    private static readonly Matrix ScaledDiscountFromLinRGB = Matrix.From(
    [
        [
            0.001200833568784504,
            0.002389694492170889,
            0.0002795742885861124,
        ],
        [
            0.0005891086651375999,
            0.0029785502573438758,
            0.0003270666104008398,
        ],
        [
            0.00010146692491640572,
            0.0005364214359186694,
            0.0032979401770712076,
        ],
    ]);

    /// <summary>
    /// YFromLinRGB represents the transformation coefficients used to calculate the luminance (Y) value from
    /// linear RGB values.
    /// </summary>
    private static readonly double[] YFromLinRGB = {0.2126, 0.7152, 0.0722};

    /// <summary>
    /// Finds the result using the given hue and y values in the CIECAM02 color appearance model.
    /// </summary>
    /// <param name="hueRad">The hue angle in radians.</param>
    /// <param name="y">The lightness value.</param>
    /// <returns>The RGBA color representation of the result.</returns>
    private RGBA FindResultViaJ(double hueRad, double y)
    {
        double j = Sqrt(y) * 11;

        double tInnerCoEff = 1 / Pow(1.64 - Pow(0.29, ViewingConditions.BackgroundYToWhitePointY), 0.73);

        double eHue = 0.25 * (Cos(hueRad + 2) + 3.8);

        double p1 = eHue * (50_000 / 13d) * ViewingConditions.Nc * ViewingConditions.Ncb;

        double hSin = Sin(hueRad);
        double hCos = Cos(hueRad);

        for (int i = 0; i < 5; i++)
        {
            double jNormalized = j / 100;

            double alpha = C is 0 || j is 0 ? 0 : C / Sqrt(jNormalized); // not transparency alpha.

            // ReSharper disable once LocalVariableHidesPrimaryConstructorParameter
            double t = Pow(alpha * tInnerCoEff, 1.0 / 0.9);

            double ac = ViewingConditions.Aw * Pow(jNormalized, 1 / ViewingConditions.C / ViewingConditions.Z);

            double p2 = ac / ViewingConditions.Nbb;

            double gamma = 23 * (p2 + 0.305) * t / (23 * p1 + 11 * t * hCos + 108 * t * hSin);

            // ReSharper disable once LocalVariableHidesPrimaryConstructorParameter
            double a = gamma * hCos;
            double b = gamma * hSin;

            const double chromaticAdaptationDivisor = 1403;
            double rA = (460 * p2 + 451 * a + 288 * b) / chromaticAdaptationDivisor;
            double gA = (460 * p2 - 891 * a - 261 * b) / chromaticAdaptationDivisor;
            double bA = (460 * p2 - 220 * a - 6300 * b) / chromaticAdaptationDivisor;

            double rCScaled = InverseChromaticAdaptation(rA);
            double gCScaled = InverseChromaticAdaptation(gA);
            double bCScaled = InverseChromaticAdaptation(bA);

            Vector scaled = Vector.From([rCScaled, gCScaled, bCScaled]);
            Vector linRGB = LinRGBFromScaledDiscount.Multiply(scaled);

            if (linRGB[0] < 0 || linRGB[1] < 0 || linRGB[2] < 0) return (RGBA)0;

            double kR = YFromLinRGB[0];
            double kG = YFromLinRGB[1];
            double kB = YFromLinRGB[2];

            double fnj = kR * linRGB[0] + kG * linRGB[1] + kB * linRGB[2];

            if (fnj <= 0) return (RGBA)0;

            if (i is 4 || Abs(fnj - y) < 2e-3)
            {
                if (linRGB[0] > 100.01 || linRGB[1] > 100.01 || linRGB[2] > 100.01) return (RGBA)0;
                return RGBAFromLinRGB(linRGB);
            }

            j -= (fnj - y) * j / (2 * fnj);
        }

        return (RGBA)0;
    }

    /// <summary>
    /// Converts the current color to RGBA format.
    /// </summary>
    /// <returns>The RGBA representation of the color.</returns>
    [Pure]
    public RGBA ToRGBA()
    {
        if (C < Precision) return RGBAFromTone();

        switch (T)
        {
            case < Precision:
                return new RGBA(0, 0, 0);
            case > 100 - Precision:
                return new RGBA(255, 255, 255);
        }

        double hueRad = H / 180 * double.Pi;
        double y = YFromTone();
        
        RGBA exact = FindResultViaJ(hueRad, y);
        if ((uint)exact is not 0) return exact;

        Vector linRGB = BisectToLimit(y, hueRad);
        return RGBAFromLinRGB(linRGB);
    }

    /// <summary>
    /// An array containing critical planes.
    /// </summary>
    private static readonly double[] CriticalPlanes = {
        0.015176349177441876, 0.045529047532325624, 0.07588174588720938, 0.10623444424209313,  0.13658714259697685,  0.16693984095186062,
        0.19729253930674434,  0.2276452376616281,   0.2579979360165119,  0.28835063437139563,  0.3188300904430532,   0.350925934958123,
        0.3848314933096426,   0.42057480301049466,  0.458183274052838,   0.4976837250274023,   0.5391024159806381,   0.5824650784040898,
        0.6277969426914107,   0.6751227633498623,   0.7244668422128921,  0.775853049866786,    0.829304845476233,    0.8848452951698498,
        0.942497089126609,    1.0022825574869039,   1.0642236851973577,  1.1283421258858297,   1.1946592148522128,   1.2631959812511864,
        1.3339731595349034,   1.407011200216447,    1.4823302800086415,  1.5599503113873272,   1.6398909516233677,   1.7221716113234105,
        1.8068114625156377,   1.8938294463134073,   1.9832442801866852,  2.075074464868551,    2.1693382909216234,   2.2660538449872063,
        2.36523901573795,     2.4669114995532007,   2.5710888059345764,  2.6777882626779785,   2.7870270208169257,   2.898822059350997,
        3.0131901897720907,   3.1301480604002863,   3.2497121605402226,  3.3718988244681087,   3.4967242352587946,   3.624204428461639,
        3.754355295633311,    3.887192587735158,    4.022731918402185,   4.160988767090289,    4.301978482107941,    4.445716283538092,
        4.592217266055746,    4.741496401646282,    4.893568542229298,   5.048448422192488,    5.20615066083972,     5.3666897647573375,
        5.5300801301023865,   5.696336044816294,    5.865471690767354,   6.037501145825082,    6.212438385869475,    6.390297286737924,
        6.571091626112461,    6.7548350853498045,   6.941541251256611,   7.131223617812143,    7.323895587840543,    7.5195704746346665,
        7.7182615035334345,   7.919981813454504,    8.124744458384042,   8.332562408825165,    8.543448553206703,    8.757415699253682,
        8.974476575321063,    9.194643831691977,    9.417930041841839,   9.644347703669503,    9.873909240696694,    10.106627003236781,
        10.342513269534024,   10.58158024687427,    10.8238400726681,    11.069304815507364,   11.317986476196008,   11.569896988756009,
        11.825048221409341,   12.083451977536606,   12.345119996613247,  12.610063955123938,   12.878295467455942,   13.149826086772048,
        13.42466730586372,    13.702830557985108,   13.984327217668513,  14.269168601521828,   14.55736596900856,    14.848930523210871,
        15.143873411576273,   15.44220572664832,    15.743938506781891,  16.04908273684337,    16.35764934889634,    16.66964922287304,
        16.985093187232053,   17.30399201960269,    17.62635644741625,   17.95219714852476,    18.281524751807332,   18.614349837764564,
        18.95068293910138,    19.290534541298456,   19.633915083172692,  19.98083495742689,    20.331304511189067,   20.685334046541502,
        21.042933821039977,   21.404114048223256,   21.76888489811322,   22.137256497705877,   22.50923893145328,    22.884842241736916,
        23.264076429332462,   23.6469514538663,     24.033477234264016,  24.42366364919083,    24.817520537484558,   25.21505769858089,
        25.61628489293138,    26.021211842414342,   26.429848230738664,  26.842203703840827,   27.258287870275353,   27.678110301598522,
        28.10168053274597,    28.529008062403893,   28.96010235337422,   29.39497283293396,    29.83362889318845,    30.276079891419332,
        30.722335150426627,   31.172403958865512,   31.62629557157785,   32.08401920991837,    32.54558406207592,    33.010999283389665,
        33.4802739966603,     33.953417292456834,   34.430438229418264,  34.911345834551085,   35.39614910352207,    35.88485700094671,
        36.37747846067349,    36.87402238606382,    37.37449765026789,   37.87891309649659,    38.38727753828926,    38.89959975977785,
        39.41588851594697,    39.93615253289054,    40.460400508064545,  40.98864111053629,    41.520882981230194,   42.05713473317016,
        42.597404951718396,   43.141702194811224,   43.6900349931913,    44.24241185063697,    44.798841244188324,   45.35933162437017,
        45.92389141541209,    46.49252901546552,    47.065252796817916,  47.64207110610409,    48.22299226451468,    48.808024568002054,
        49.3971762874833,     49.9904556690408,     50.587870934119984,  51.189430279724725,   51.79514187861014,    52.40501387947288,
        53.0190544071392,     53.637271562750364,   54.259673423945976,  54.88626804504493,    55.517063457223934,   56.15206766869424,
        56.79128866487574,    57.43473440856916,    58.08241284012621,   58.734331877617365,   59.39049941699807,    60.05092333227251,
        60.715611475655585,   61.38457167773311,    62.057811747619894,  62.7353394731159,     63.417162620860914,   64.10328893648692,
        64.79372614476921,    65.48848194977529,    66.18756403501224,   66.89098006357258,    67.59873767827808,    68.31084450182222,
        69.02730813691093,    69.74813616640164,    70.47333615344107,   71.20291564160104,    71.93688215501312,    72.67524319850172,
        73.41800625771542,    74.16517879925733,    74.9167682708136,    75.67278210128072,    76.43322770089146,    77.1981124613393,
        77.96744375590167,    78.74122893956174,    79.51947534912904,   80.30219030335869,    81.08938110306934,    81.88105503125999,
        82.67721935322541,    83.4778813166706,     84.28304815182372,   85.09272707154808,    85.90692527145302,    86.72564993000343,
        87.54890820862819,    88.3767072518277,     89.2090541872801,    90.04595612594655,    90.88742016217518,    91.73345337380438,
        92.58406282226491,    93.43925555268066,    94.29903859396902,   95.16341895893969,    96.03240364439274,    96.9059996312159,
        97.78421388448044,    98.6670533535366,     99.55452497210776
    };

    #endregion

    #region From RGB

    /// <summary>
    /// Calculates the hue and chroma values from the given RGBA color.
    /// </summary>
    /// <param name="rgba">The RGBA color value to calculate the hue and chroma from.</param>
    /// <returns>A tuple containing the hue and chroma values.</returns>
    private static (double hue, double chroma) HueChromaFromRGBA(RGBA rgba)
    {
        double redL = Linearized(rgba.R);
        double greenL = Linearized(rgba.G);
        double blueL = Linearized(rgba.B);
        
        double x = 0.41233895 * redL + 0.35762064 * greenL + 0.18051042 * blueL;
        double y = 0.2126 * redL + 0.7152 * greenL + 0.0722 * blueL;
        double z = 0.01932141 * redL + 0.11916382 * greenL + 0.95034478 * blueL;
        
        // XYZ to cone responses
        double rCone = 0.401288 * x + 0.650173 * y - 0.051461 * z;
        double gCone = -0.250268 * x + 1.204414 * y + 0.045854 * z;
        double bCone = -0.002079 * x + 0.048952 * y + 0.953127 * z;

        double rDiscount = ViewingConditions.RgbD[0] * rCone;
        double gDiscount = ViewingConditions.RgbD[1] * gCone;
        double bDiscount = ViewingConditions.RgbD[2] * bCone;

        double rAf = Pow(ViewingConditions.Fl * Abs(rDiscount) / 100, 0.42);
        double gAf = Pow(ViewingConditions.Fl * Abs(gDiscount) / 100, 0.42);
        double bAf = Pow(ViewingConditions.Fl * Abs(bDiscount) / 100, 0.42);
        
        // Chromatic adaptations
        double rA = SignOf(rDiscount) * 400 * rAf / (rAf + 27.13);
        double gA = SignOf(gDiscount) * 400 * gAf / (gAf + 27.13);
        double bA = SignOf(bDiscount) * 400 * bAf / (bAf + 27.13);
        
        // Redness-Greenness
        double a = (11 * rA + -12 * gA + bA) / 11;
        double b = (rA + gA - 2 * bA) / 9; // const b = (rA + gA - 2.0 * bA) / 9.0;
        double u = (20 * rA + 20 * gA + 21 * bA) / 20;
        double p2 = (40 * rA + 20 * gA + bA) / 20;

        double radians = Atan2(b, a);
        double degrees = radians * 180 / double.Pi;
        double hue = Color.SanitizeDegrees(degrees);
        double ac = p2 * ViewingConditions.Nbb;

        double j = 100 * Pow(ac / ViewingConditions.Aw, ViewingConditions.C * ViewingConditions.Z);

        double huePrime = hue < 20.14 ? hue + 360 : hue;
        double eHue = 0.25 * (Cos(huePrime * double.Pi / 180 + 2) + 3.8);

        double p1 = 50_000 / 13d * eHue * ViewingConditions.Nc * ViewingConditions.Ncb;
        double t = p1 * Sqrt(a * a + b * b) / (u + 0.305);
        double alpha = Pow(t, 0.9) * Pow(1.64 - Pow(0.29, ViewingConditions.BackgroundYToWhitePointY), 0.73); // not transparency

        double chroma = alpha * Sqrt(j / 100);

        return (hue, chroma);
    }

    /// <summary>
    /// Converts an RGBA color to an HCTA color.
    /// </summary>
    /// <param name="rgba">The RGBA color to convert.</param>
    /// <returns>The converted HCTA color.</returns>
    public static HCTA FromRGBA(RGBA rgba)
    {
        (double h, double c) = HueChromaFromRGBA(rgba);
        double t = ToneFromRGBA(rgba);

        return new HCTA(h, c, t, rgba.A);
    }

    #endregion

    #region Equals

    public bool Equals(HCTA? other) => other is not null && (H, C, T, A) == (other.H, other.C, other.T, other.A);
    
    public bool Equals(IRGB? other) => ToRGBA().Equals(other);

    public bool Equals(Color? other) => ((IRGB)ToRGBA()).Equals(other);

    public static bool operator ==(HCTA? left, HCTA? right) => left is not null && left.Equals(right);
    public static bool operator !=(HCTA? left, HCTA? right) => left is not null && !left.Equals(right);
    
    public static bool operator ==(HCTA? left, Color? right) => left is not null && left.Equals(right);
    public static bool operator !=(HCTA? left, Color? right) => left is not null && !left.Equals(right);
    
    #endregion

    public int MaxChroma() => MaxChroma(H, T);
    public double ExactMaxChroma(int accuracy) => ExactMaxChroma(H, T, accuracy);

    /// <summary>
    /// Returns an approximate max chroma of a hue-tone pair, rounded up to the next greatest integer.
    /// This will not be the exact max chroma, but it will be close enough for almost every use case.
    /// For a more exact method, use <see cref="ExactMaxChroma(double, double, int)"/>
    /// </summary>
    public static int MaxChroma(double hue, double tone)
    {
        if (tone is <= 0 or >= 100) return 0;
        
        const double maxChroma = 150;
        // using the max possible chroma and then converting it to and from RGBA will bring it to a rough max chroma
        HCTA max = FromRGBA(new HCTA(hue, maxChroma, tone).ToRGBA()); 
        // this can't be assumed to be accurate, or true either, so using ceiling can make it at least correct as a max
        return (int)Ceiling(max.C);
    }

    public static double ExactMaxChroma(double hue, double tone, int precision)
    {
        if (tone is <= 0 or >= 100) return 0;
        
        double estimate = MaxChroma(hue, tone); // using a non-precise method to get an estimate
        if (precision <= 0) return estimate;

        double iterAmount = 1 / (10d * precision);
        RGBA? last = null;
        for (double chroma = estimate - 1; chroma < estimate + 0.5; chroma += iterAmount)
        {
            last ??= new HCTA(hue, chroma - iterAmount, tone).ToRGBA();
            RGBA current = new HCTA(hue, chroma, tone).ToRGBA();

            if (last == current) return chroma;
            
            last = current;
        }

        return estimate + 0.5;
    }
    
    public override string ToString() => $"hcta({H}, {C}, {T}, {A}%)";

    public override int GetHashCode()
    {
        return HashCode.Combine(_hashCode);
    }
}