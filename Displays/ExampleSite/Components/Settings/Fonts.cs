using System.Diagnostics.Contracts;
using MaterialDesign.Web.Fonts;
using MaterialDesign.Web.Fonts.Enums;
using static MaterialDesign.Web.Fonts.Enums.FontWeightValue;

namespace ExampleSite.Components.Settings;

public static class Fonts
{
    public static Dictionary<FontWeightValue, FontFace> Merriweather { get; }
    public static Dictionary<FontWeightValue, FontFace> Montserrat { get; }
    public static Dictionary<OpenSansOptions, Dictionary<FontWeightValue, FontFace>> OpenSans { get; }
    public static Dictionary<FontWeightValue, FontFace> PlayfairDisplay { get; }
    public static Dictionary<FontWeightValue, FontFace> Poppins { get; }
    public static Dictionary<FontWeightValue, FontFace> Raleway { get; }
    public static Dictionary<FontWeightValue, FontFace> Roboto { get; }

    public static FontFace[] AllFontFaces { get; }
    
    static Fonts()
    {
        Merriweather = CreateFontFaces(FontFamily.Merriweather, [Light, Regular, Bold, Black]);
        Montserrat = CreateFontFaces(FontFamily.Montserrat, [Thin, ExtraLight, Light, Regular, Medium, SemiBold, Bold, ExtraBold, Black]);
        
        FontWeightValue[] openSansWeights = [Light, Regular, Medium, SemiBold, Bold, ExtraBold];
        Dictionary<FontWeightValue, FontFace> openSans = CreateFontFaces(FontFamily.OpenSans, openSansWeights);
        Dictionary<FontWeightValue, FontFace> openSansSemi = CreateFontFaces(FontFamily.OpenSans, openSansWeights, OpenSansOptions.SemiCondensed.ToString());
        Dictionary<FontWeightValue, FontFace> openSansCondensed = CreateFontFaces(FontFamily.OpenSans, openSansWeights, OpenSansOptions.Condensed.ToString());
        OpenSans = new Dictionary<OpenSansOptions, Dictionary<FontWeightValue, FontFace>>
        {
            { OpenSansOptions.Default, openSans },
            { OpenSansOptions.SemiCondensed, openSansSemi },
            { OpenSansOptions.Condensed, openSansCondensed }
        };

        PlayfairDisplay = CreateFontFaces(FontFamily.PlayfairDisplay, [Regular, Medium, SemiBold, Bold, Black]);
        Poppins = CreateFontFaces(FontFamily.Poppins, [Thin, ExtraLight, Light, Regular, Medium, SemiBold, Bold, ExtraBold, Black]);
        Raleway = CreateFontFaces(FontFamily.Raleway, [Thin, ExtraLight, Light, Regular, Medium, SemiBold, Bold, ExtraBold, Black]);
        Roboto = CreateFontFaces(FontFamily.Roboto, [Thin, Light, Regular, Medium, Bold, Black]);

        AllFontFaces =
        [
            ..Merriweather.Values, ..Montserrat.Values, ..openSans.Values, ..openSansSemi.Values,
            ..openSansCondensed.Values, ..PlayfairDisplay.Values, ..Poppins.Values, ..Raleway.Values, ..Roboto.Values
        ];
    }
    
    [Pure]
    public static FontFace[] FilterFontFaces(FontFamily? family, FontWeightValue? weight, 
        bool? serif)
    {
        FontFace[] values = AllFontFaces;
        
        // use StartsWith() since GetFontFamily() appends any options to the family and this method does not use options. Otherwise, it's similar to an Equals() operation.
        if (family is not null) values = values.Where(x => x.Family.Trim('"').StartsWith(GetFontFamily(family.Value))).ToArray();

        if (weight is not null) values = values
                .Where(x => x.Weight == weight.Value.ToString().ToLowerInvariant().Replace("regular", "normal"))
                .ToArray();

        if (serif is not null) values = values.Where(MatchesSerif).ToArray();

        return values;

        // ReSharper disable StringLiteralTypo
        bool MatchesSerif(FontFace x) => 
            (x.Family.StartsWith("\"Playfair Display\"") || x.Family.StartsWith("\"Merriweather\"")) == serif.Value;
        // ReSharper restore StringLiteralTypo
    }
    
    public enum FontFamily
    {
        Merriweather,
        Montserrat,
        OpenSans,
        PlayfairDisplay,
        Poppins,
        Raleway,
        Roboto
    }
    // ReSharper restore IdentifierTypo

    public enum OpenSansOptions
    {
        Default,
        SemiCondensed,
        Condensed
    }

    private static Dictionary<FontWeightValue, FontFace> CreateFontFaces(FontFamily family, 
        IEnumerable<FontWeightValue> weights, string? option = null)
    {
        return weights.ToDictionary(weight => weight, weight => GetFontFace(family, weight, option));
    }
    
    private static FontFace GetFontFace(FontFamily family, FontWeightValue weight, string? option = null)
    {
        if (weight is Lighter or Bolder)
            throw new ArgumentException("Font Weight parameter `weight` cannot be Lighter or Bolder.", nameof(weight));
        
        FontFaceBuilder builder = new();

        string familyString = GetFontFamily(family, option);

        builder
            .FontFamily(familyString)
            .FontWeight(new FontWeight(weight))
            .FontSource(SourceBuilder);

        return builder.Build();
        
        void SourceBuilder(FontSourceBuilder sourceBuilder) => 
            sourceBuilder.AddUrlSource(GetFontUri(family, weight, option), FontFormat.TrueType);
    }

    public static string GetFontFamily(FontFamily family, string? option = null)
    {
        string text = $"{family}{option}"; // null in string interpolation adds no content.
        
        string newText = string.Empty;
        newText += text[0];
        
        for (int i = 1; i < text.Length; i++)
        {
            if (char.IsUpper(text[i]) && text[i - 1] != ' ')
                newText += ' ';
            newText += text[i];
        }
        
        return newText;
    }

    private static string GetFontUri(FontFamily family, FontWeightValue weight, string? option = null) => 
        option is null
        ? $"fonts/{family}/{family}-{weight}.ttf"
        : $"fonts/{family}/{option}/{family}_{option}-{weight}.ttf"; // only really applies to Open Sans, but supports new fonts that require this.
}