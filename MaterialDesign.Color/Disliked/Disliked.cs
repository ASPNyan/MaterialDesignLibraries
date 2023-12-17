namespace MaterialDesign.Color.Disliked;

/// <summary>
///  Checks and/or fixes universally disliked colors.
/// 
/// Color science studies of color preference indicate universal distaste for
/// dark yellow-greens, and also show this is correlated to distaste for
/// biological waste and rotting food.
/// 
/// See Palmer and Schloss, 2010 or Schloss and Palmer's Chapter 21 in Handbook
/// of Color Psychology (2015).
/// </summary>
public static class Disliked
{
    /// <summary>
    /// Disliked is defined as a dark yellow-green that is not neutral.
    /// </summary>
    /// <param name="hcta">The color to be tested</param>
    /// <returns>Whether the color is disliked.</returns>
    public static bool IsDisliked(this HCTA hcta)
    {
        bool huePasses = Math.Round(hcta.H) is >= 90 and <= 111;
        bool chromaPasses = Math.Round(hcta.C) > 16;
        bool tonePasses = Math.Round(hcta.T) < 65;

        return huePasses && chromaPasses && tonePasses;
    }

    /// <summary>
    /// If a color is disliked, lightens it to make it likable. The original color is not modified.
    /// </summary>
    /// <param name="hcta">The color to be tested and fixed where needed.</param>
    /// <returns>The original color if it is not disliked; otherwise, the fixed color.</returns>
    public static HCTA FixIfDisliked(this HCTA hcta) => hcta.IsDisliked() ? new HCTA(hcta.H, hcta.C, 70) : hcta;
}