using System.Diagnostics.Contracts;
using MaterialDesign.Color.Palettes;

namespace MaterialDesign.Color.Extensions;

public static class CorePaletteExtensions
{
    [Pure]
    public static CorePalette SimulateColorDeficiency(this CorePalette palette, float severity, ColorDeficiency deficiency) 
        => new(palette.Origin.SimulateColorDeficiency(severity, deficiency));
}