using System.Numerics;

namespace MaterialDesign.Color.Colorspaces;

public static class ViewingConditions
{
    public const double AdaptingLuminance = 11.725676537;
    public const double BackgroundLStar = 50;
    public const double Surround = 2;
    public const bool DiscountingIlluminant = false;
    public const double BackgroundYToWhitePointY = 0.184186503;
    public const double Aw = 29.981000900;
    public const double Nbb = 1.016919255;
    public const double Ncb = Nbb;
    public const double C = 0.689999998;
    public const double Nc = 1;
    public const double Fl = 0.388481468;
    public const double FlRoot = 0.789482653;
    public const double Z = 1.909169555;

    
    public static readonly Vector3 WhitePoint = new(95.047f, 100.0f, 108.883f);
    public static readonly double N = HCTA.YFromTone(BackgroundLStar) / WhitePoint[1];
    public static readonly Vector3 RgbD = new(1.021177769f, 0.986307740f, 0.933960497f);
}