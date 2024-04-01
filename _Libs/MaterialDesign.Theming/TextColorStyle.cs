// ReSharper disable CommentTypo
namespace MaterialDesign.Theming;

public enum TextColorStyle
{
    /// <summary>
    /// Uses the chosen color from the <see cref="IScheme"/> and applies it to the text.
    /// </summary>
    Colored,
    /// <summary>
    /// Converts the related colored text to either black or white, depending on the color's <see cref="HCTA.T">Tone</see>.
    /// Black &amp; White text can cause visual artifacts called halation, which makes text appear blurry. If this
    /// happens, try using <see cref="GreyAndGrey"/> instead.
    /// </summary>
    BlackAndWhite,
    /// <summary>
    /// Similar to <see cref="BlackAndWhite">Black &amp; White</see> text, but using dark and light greys
    /// instead of black and white to prevent halation (see more
    /// <a href="https://uxmovement.com/content/why-you-should-never-use-pure-black-for-text-or-backgrounds/">here</a>).
    /// </summary>
    GreyAndGrey
}