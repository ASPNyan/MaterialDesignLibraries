// This file provides XML documentation for the MdIconText component.
using MaterialDesign.Web.Exceptions;

namespace MaterialDesign.Icons.Components;

// ReSharper disable InvalidXmlDocComment
/// <summary>
/// <inheritdoc cref="MdIcon"/>.
/// Alongside Material Symbols, this component creates a horizontal (and clickable when desired) pair of symbol and text.
/// </summary>
/// <inheritdoc cref="MdIcon"/>
/// <param name="Text">The text added after the content, throws a <see cref="ParameterException"/> if not provided.</param>
/// <param name="Href">The href provided to the anchor tag. If the symbol/text pair should not be a link, do not set.</param>
// ReSharper restore InvalidXmlDocComment
public partial class MdIconText : MdIcon;