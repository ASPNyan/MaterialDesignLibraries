namespace MaterialDesign.Blend;

/// <summary>
/// This class provides methods for blending colors using various color models.
/// </summary>
public static class Blend
{
    /// <summary>
    /// Harmonizes two colors based on their hues.
    /// </summary>
    /// <param name="designColor">Base color for harmonization.</param>
    /// <param name="sourceColor">Color to be harmonized with the base color.</param>
    /// <returns>New color after harmonization.</returns>
    public static HCTA Harmonize(HCTA designColor, HCTA sourceColor)
    {
        double differenceDegrees = Color.DifferenceDegrees(designColor.H, sourceColor.H);
        double rotationDegrees = Math.Min(differenceDegrees * 0.5, 15);

        double outputHue =
            Color.SanitizeDegrees(designColor.H + rotationDegrees * RotationDirection(designColor.H, sourceColor.H));

        return new HCTA(outputHue, designColor.C, designColor.T);
    }

    /// <summary>
    /// Blends the hues of two colors based on a given amount.
    /// </summary>
    /// <param name="from">Base color for the blend operation.</param>
    /// <param name="to">Color to be blended with the base color.</param>
    /// <param name="amount">Amount for blending. Should be between 0 and 1.</param>
    /// <returns>New color after blending.</returns>
    public static HCTA HCTAHue(HCTA from, HCTA to, double amount)
    {
        HCTA ucs = ViaCAM16Ucs(from, to, amount);
        CAM16 ucsCam = CAM16.FromRGBA(ucs.ToRGBA());
        CAM16 fromCam = CAM16.FromRGBA(from.ToRGBA());

        HCTA blended = new HCTA(ucsCam.Hue, fromCam.Chroma, from.T);
        return blended;
    }

    /// <summary>
    /// Blends two colors based on a given amount using the CAM16-UCS color model.
    /// </summary>
    /// <returns>New color after blending.</returns>
    public static HCTA ViaCAM16Ucs(HCTA from, HCTA to, double amount) =>
        HCTA.FromRGBA(ViaCAM16Ucs(from.ToRGBA(), to.ToRGBA(), amount));

    /// <summary>
    /// Blends two colors based on a given amount using the CAM16-UCS color model.
    /// </summary>
    /// <returns>New color after blending.</returns>
    public static RGBA ViaCAM16Ucs(RGBA from, RGBA to, double amount)
    {
        CAM16 fromCam = CAM16.FromRGBA(from);
        CAM16 toCam = CAM16.FromRGBA(to);

        double fromJ = fromCam.JStar;
        double fromA = fromCam.AStar;
        double fromB = fromCam.BStar;
        
        double toJ = toCam.JStar;
        double toA = toCam.AStar;
        double toB = toCam.BStar;

        double jStar = fromJ + (toJ - fromJ) * amount;
        double aStar = fromA + (toA - fromA) * amount;
        double bStar = fromB + (toB - fromB) * amount;

        return CAM16.FromUcs(jStar, aStar, bStar).ToRGBA();
    }
    
    /// <summary>
    /// Determines the rotation direction for hue blending.
    /// </summary>
    /// <param name="from">Initial hue angle.</param>
    /// <param name="to">Final hue angle.</param>
    /// <returns>Direction of rotation (1 for anticlockwise, -1 for clockwise).</returns>
    private static int RotationDirection(double from, double to) => Color.SanitizeDegrees(to - from) <= 180 ? 1 : -1;
}
