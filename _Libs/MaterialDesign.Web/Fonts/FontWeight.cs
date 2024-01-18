using MaterialDesign.Web.Fonts.Enums;
using static MaterialDesign.Web.Fonts.FontWeightClassNames;

namespace MaterialDesign.Web.Fonts;

public readonly struct FontWeight
{
    private string Lower { get; }
    private string Upper { get; }
    
    private static string GetWeightString(FontWeightValue weight)
    {
        return weight switch
        {
            FontWeightValue.Lighter => "lighter",
            FontWeightValue.Bolder => "bolder",
            FontWeightValue.Regular => "normal",
            _ => (int)weight % 100 == 0 ? weight.ToString().ToLower() : ((int)weight).ToString()
        };
    }

    private static string GetWeightString(int weight)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(weight, 1, nameof(weight));
        ArgumentOutOfRangeException.ThrowIfGreaterThan(weight, 1000, nameof(weight));
        return $"{weight}";
    }

    public FontWeight(FontWeightValue value)
    {
        Lower = Upper = GetWeightString(value);
    }
    
    public FontWeight(int value)
    {
        Lower = Upper = GetWeightString(value);
    }
    
    public FontWeight(FontWeightValue lower, FontWeightValue upper)
    {
        Lower = GetWeightString(lower);
        Upper = GetWeightString(upper);
    }

    public FontWeight(int lower, int upper)
    {
        Lower = GetWeightString(lower);
        Upper = GetWeightString(upper);
    }

    public FontWeight(FontWeightValue lower, int upper)
    {
        Lower = GetWeightString(lower);
        Upper = GetWeightString(upper);
    }
    
    public FontWeight(int lower, FontWeightValue upper)
    {
        Lower = GetWeightString(lower);
        Upper = GetWeightString(upper);
    }
    
    
    private static string GetCSSValue(string value)
    {
        if (int.TryParse(value, out int intValue))
            return $"{intValue}";
        
        if (Enum.TryParse(value, true, out FontWeightValue enumValue))
        {
            return enumValue switch
            {
                FontWeightValue.Lighter => "lighter",
                FontWeightValue.Bolder => "bolder",
                FontWeightValue.Regular or FontWeightValue.Normal => "normal",
                FontWeightValue.Bold => "bold",
                _ => $"{(int)enumValue}"
            };
        }

        throw new Exception("Invalid weight provided.");
    }

    public override string ToString()
    {
        if (Lower == Upper) return GetCSSValue(Lower);
        
        return $"{GetCSSValue(Lower)} {GetCSSValue(Upper)}";
    }

    /// <remarks>
    /// Class names can be modified at <see cref="FontWeightClassNames"/>.
    /// </remarks>
    public static string ClassNameFromCSSValue(string cssValue) =>
        cssValue.Replace(' ', '-').Replace("lighter", Lighter, StringComparison.InvariantCultureIgnoreCase)
            .Replace("bolder", Bolder, StringComparison.InvariantCultureIgnoreCase)
            .Replace("100", Thin).Replace("200", ExtraLight)
            .Replace("300", Light).Replace("normal", Normal, StringComparison.InvariantCultureIgnoreCase)
            .Replace("regular", Regular, StringComparison.InvariantCultureIgnoreCase).Replace("500", Medium)
            .Replace("600", SemiBold).Replace("bold", Bold, StringComparison.InvariantCultureIgnoreCase)
            .Replace("800", ExtraBold).Replace("900", Black);
}