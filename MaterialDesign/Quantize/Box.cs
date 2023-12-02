namespace MaterialDesign.Quantize;

/// <summary>
/// Represents a box with color and volume properties.
/// </summary>
/// <param name="R0">The lower boundary of the red color channel.</param>
/// <param name="R1">The upper boundary of the red color channel.</param>
/// <param name="G0">The lower boundary of the green color channel.</param>
/// <param name="G1">The upper boundary of the green color channel.</param>
/// <param name="B0">The lower boundary of the blue color channel.</param>
/// <param name="B1">The upper boundary of the blue color channel.</param>
/// <param name="Vol">The volume of the box.</param>
public readonly record struct Box(
    int R0 = 0,
    int R1 = 0,
    int G0 = 0,
    int G1 = 0,
    int B0 = 0,
    int B1 = 0,
    int Vol = 0);