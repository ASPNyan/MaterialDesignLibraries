// This file provides XML docs for the MdIcon component

namespace MaterialDesign.Icons.Components;

// ReSharper disable InvalidXmlDocComment
/// <summary>
/// Provides support for <a href="https://fonts.google.com/icons">Material Symbols</a> as a Razor Component.
/// Displaying the icons does require ligatures support, which is in every major desktop and mobile browser.
/// For support data, see CanIUse's page here: https://caniuse.com/mdn-css_properties_font-variant-ligatures
/// </summary>
/// <param name="Icon">The name of the material icon, found at https://fonts.google.com/icons</param>
/// <param name="Scale">The scale of the Icon, applied as a multiplier.</param>
/// <param name="UserSelect">
/// Modifies the CSS <a href="https://developer.mozilla.org/en-US/docs/Web/CSS/user-select">user-select</a> property
/// on the Icon to disable text highlighting when false, and enable it when true. False by default.
/// </param>
/// <exception cref="VoidElementException">Thrown if Child Content is passed to the component.</exception>
/// <inheritdoc cref="MdIconConfigComponent"/>
// ReSharper restore InvalidXmlDocComment
public partial class MdIcon;