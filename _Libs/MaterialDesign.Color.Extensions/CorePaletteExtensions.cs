using System.Diagnostics.Contracts;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Extensions;

public static class CorePaletteExtensions
{
    /// <summary>
    /// Creates a new <see cref="CorePalette"/> with a copy of the origin color with the selected CVD filter.
    /// Does not modify the original.
    /// </summary>
    [Pure]
    public static CorePalette SimulateColorDeficiency(this CorePalette palette, float severity, ColorDeficiency deficiency) 
        => new(palette.Origin.SimulateColorDeficiency(severity, deficiency));
}