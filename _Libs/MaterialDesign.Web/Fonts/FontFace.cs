using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Web.Fonts;

public readonly struct FontFace
{
    public required string Family { get; init; }
    public required string Weight { get; init; }
    public required string SourceString { get; init; }

    /// <summary>
    /// <inheritdoc cref="ToString"/> Beware of potential XSS attacks if this is used on a <see cref="FontFace"/>
    /// created with user inputs without checking for potential issues, like closing script tags (&lt;/style&gt;) when
    /// using this in HTML. Sanitized inputs should always be used in scenarios where user input is displayed in HTML.
    /// </summary>
    public MarkupString ToMarkupString()
    {
        return (MarkupString)$$"""
                               @font-face {
                                 font-family: {{Family}};
                                 font-weight: {{Weight}};
                                 {{SourceString}}
                               }
                               
                               """;
    }

    public static implicit operator MarkupString(FontFace fontFace) => fontFace.ToMarkupString();
    
    public override string ToString() => ToMarkupString().Value;
}