using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace MaterialDesign.Icons;

public class MdConfigurationHeadContent : ComponentBase
{
    [Inject] public MdIconConfiguration? IconConfig { get; set; }

    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        for (int i = 0; i < 3; i++)
        {
            builder.OpenElement(i, "link");
            builder.AddAttribute(i + 1, "rel", "stylesheet");
            builder.AddAttribute(i + 3, "href", GenerateMaterialIconsUrl((MdIconLineStyle)i));
            builder.CloseElement();
        }
    }

    private string GenerateMaterialIconsUrl(MdIconLineStyle style)
        => $"https://fonts.googleapis.com/css2?family=Material+Symbols+{style.ToString()}" +
           $"{IconConfig?.GetFontConfigurationString()}";
}