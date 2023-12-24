namespace MaterialDesign.Color.Contrast;

public readonly record struct ContrastRatio(double Ratio)
{
    public static ContrastRatio None => new(1);
    public static ContrastRatio Shade => new(1.5);
    public static ContrastRatio Icon => new(3);
    public static ContrastRatio Text => new(4.5);
    public static ContrastRatio Emphasis => new(7);
    public static ContrastRatio HighEmphasis => new(13);
    public static ContrastRatio BlackAndWhite => new(21);
}