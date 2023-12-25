using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace MaterialDesign.Icons.Components;

#pragma warning disable BL0007
// ReSharper disable InvalidXmlDocComment
/// <summary>
/// Base class to <see cref="MdIcon"/> and <see cref="MdIconConfig"/>, adding support for stylising Material Icons with
/// their four variable axes, plus the line style of the icons. This class cannot be rendered itself.
/// </summary>
/// <param name="Fill">
/// Fill gives you the ability to modify the default icon style. A single icon can render both
/// unfilled and filled states. To convey a state transition, use the fill axis for animation or interaction.
/// The values are 0 for default or 1 for completely filled. Along with the weight axis,
/// the fill also impacts the look of the icon.
/// </param>
/// <param name="Weight">
/// Weight defines the symbol's stroke weight, with a range of weights between thin (lower) and bold (higher).
/// Weight can also affect the overall size of the symbol. Weight ranges from 100 to 700 inclusively.
/// </param>
/// <param name="Grade">
/// Weight and grade affect a symbol's thickness. Adjustments to grade are more granular than adjustments to weight
/// and have a small impact on the size of the symbol. Grade is also available in some text fonts. You can match grade
/// levels between text and symbols for a harmonious visual effect. For example, if the text font has a -25 grade value,
/// the symbols can match it with a suitable value, say -25. You can use grade for different needs:
/// Low emphasis (e.g. -25 grade): To reduce glare for a light symbol on a dark background, use a low grade.
/// High emphasis (e.g. 200 grade): To highlight a symbol, increase the positive grade.
/// Grade ranges from -25 to 200 inclusively.
/// </param>
/// <param name="OpticalSize">
/// For the image to look the same at different sizes, the stroke weight (thickness) changes as the icon size scales.
/// Optical Size offers a way to automatically adjust the stroke weight when you increase or decrease the symbol size.
/// Optical Size ranges from 20 to 48 inclusively.
/// </param>
// ReSharper restore InvalidXmlDocComment
public abstract class MdIconConfigComponent : ComponentBase
{
    [Inject, NotNull] public MdIconConfiguration? MaterialIconConfiguration { get; set; }
    
    [CascadingParameter(Name = nameof(CascadingIconConfig))] 
    public MdIconConfigComponent? CascadingIconConfig { get; set; }

    private bool? _fill;
    /// <summary>
    /// Fill gives you the ability to modify the default icon style. A single icon can render both
    /// unfilled and filled states. To convey a state transition, use the fill axis for animation or interaction.
    /// The values are 0 for default or 1 for completely filled. Along with the weight axis,
    /// the fill also impacts the look of the icon.
    /// </summary>
    [Parameter]
    public bool Fill
    {
        get => _fill ?? MaterialIconConfiguration.Fill;
        set => _fill = value;
    }

    protected bool HasLocalFill() => _fill is not null;

    private int? _weight;
    /// <summary>
    /// Weight defines the symbol's stroke weight, with a range of weights between thin (lower) and bold (higher).
    /// Weight can also affect the overall size of the symbol. Weight ranges from 100 to 700 inclusively.
    /// </summary>
    [Parameter]
    public int Weight
    {
        get => _weight ?? MaterialIconConfiguration.Weight;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 100);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 700);
            _weight = value;
        }
    }

    protected bool HasLocalWeight() => _weight is not null;

    private int? _grade;
    /// <summary>
    /// Weight and grade affect a symbol's thickness. Adjustments to grade are more granular than adjustments to weight
    /// and have a small impact on the size of the symbol. Grade is also available in some text fonts. You can match grade
    /// levels between text and symbols for a harmonious visual effect. For example, if the text font has a -25 grade value,
    /// the symbols can match it with a suitable value, say -25. You can use grade for different needs:
    /// Low emphasis (e.g. -25 grade): To reduce glare for a light symbol on a dark background, use a low grade.
    /// High emphasis (e.g. 200 grade): To highlight a symbol, increase the positive grade.
    /// Grade ranges from -25 to 200 inclusively.
    /// </summary>
    [Parameter]
    public int Grade
    {
        get => _grade ?? MaterialIconConfiguration.Grade;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, -25);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 200);
            _grade = value;
        }
    }

    protected bool HasLocalGrade() => _grade is not null;

    private int? _opticalSize;
    /// <summary>
    /// For the image to look the same at different sizes, the stroke weight (thickness) changes as the icon size scales.
    /// Optical Size offers a way to automatically adjust the stroke weight when you increase or decrease the symbol size.
    /// Optical Size ranges from 20 to 48 inclusively.
    /// </summary>
    [Parameter]
    public int OpticalSize
    {
        get => _opticalSize ?? MaterialIconConfiguration.OpticalSize;
        set
        {
            ArgumentOutOfRangeException.ThrowIfLessThan(value, 20);
            ArgumentOutOfRangeException.ThrowIfGreaterThan(value, 48);
            _opticalSize = value;
        }
    }

    protected bool HasLocalOpticalSize() => _opticalSize is not null;

    /// <summary>
    /// Line Style denotes the style of the icon, <see cref="MdIconLineStyle.Outlined"/>,
    /// <see cref="MdIconLineStyle.Rounded"/>, or <see cref="MdIconLineStyle.Sharp"/>.
    /// </summary>
    [Parameter] public MdIconLineStyle? LineStyle { get; set; }

    protected override void OnParametersSet()
    {
        if (MaterialIconConfiguration is null)
            throw new ArgumentNullException(nameof(MaterialIconConfiguration),
                "MdIconConfiguration is not in the Service Provider, please add " +
                "Services.AddDynamicMaterialIcons() or Services.AddStaticMaterialIcons()");
        
        bool fill = HasLocalFill() ? Fill : CascadingIconConfig?.Fill ?? Fill;
        int weight = HasLocalWeight() ? Weight : CascadingIconConfig?.Weight ?? Weight;
        int grade = HasLocalGrade() ? Grade : CascadingIconConfig?.Grade ?? Grade;
        int opticalSize = HasLocalOpticalSize() ? OpticalSize : CascadingIconConfig?.OpticalSize ?? OpticalSize;
        
        _localIconConfig = MdIconConfiguration.CreateStatic(fill, weight, grade, opticalSize);
    }

    private MdIconConfiguration _localIconConfig = null!;

    protected string StyleRuleString => _localIconConfig.GetFontVariationStyle();
}